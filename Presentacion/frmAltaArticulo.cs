using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using dominio;
using metodos;


namespace Presentacion
{
    public partial class frmAltaArticulo : Form
    {
        private Articulos articulo = null;

        private OpenFileDialog archivo = null;
        public frmAltaArticulo()
        {
            InitializeComponent();
        }

        public frmAltaArticulo(Articulos articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Artículo";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //Articulos art = new Articulos();
            ArticuloMetodos nuevoart = new ArticuloMetodos();

            try
            {
                if(articulo == null)
                    articulo = new Articulos();

                articulo.Codigo= txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Precio = decimal.Parse(txtPrecio.Text);
                articulo.Marca = (Marca)cboMarca.SelectedItem;
                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.ImagenUrl = txtImagen.Text;

                if(articulo.Id != 0)
                {
                    nuevoart.modificar(articulo);
                    MessageBox.Show("Articulo Modificado");
                }
                else
                {
                    nuevoart.agregar(articulo);
                    MessageBox.Show("Artículo agregado");
                }
                if(archivo != null &&  !(txtImagen.Text.ToUpper().Contains("http")))
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);
                }
                Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            MarcaMetodos articuloMetodos = new MarcaMetodos();
            CategoriaMetodos articuloCategoria = new CategoriaMetodos();

            try
            {
                cboCategoria.DataSource = articuloCategoria.listar();
                // usamos la clave valor
                cboCategoria.ValueMember = "Id"; // clave
                cboCategoria.DisplayMember = "Descrip"; // valor
                cboMarca.DataSource = articuloMetodos.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descrip";

                if(articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtPrecio.Text = articulo.Precio.ToString();
                    txtImagen.Text = articulo.ImagenUrl;
                    cargarImagen(articulo.ImagenUrl);
                    cboCategoria.SelectedValue = articulo.Categoria.Id;
                    cboMarca.SelectedValue = articulo.Marca.Id;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxArticulo.Load("https://www.fabricocina.com/wp-content/uploads/2018/06/image_large.png");
            }

        }
        private void txtImagen_Leave(object sender, EventArgs e)
        {

            cargarImagen(txtImagen.Text);
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png";
            
            if(archivo.ShowDialog() == DialogResult.OK)
            {
                txtImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);
            }
        }
    }
}
