using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace RifaSistem
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarParticipantes();
            }
        }

        protected void btnInsertar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;
            int numeroElegido;

            if (!int.TryParse(txtNumeroElegido.Text, out numeroElegido))
            {
                lblMessage.Text = "Número elegido debe ser un número entero.";
                return;
            }

            bool pago = rblPago.SelectedValue == "true";

            string connectionString = ConfigurationManager.ConnectionStrings["RifaDBConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "INSERT INTO Participantes (Nombre, Apellido, NumeroElegido, Pago) VALUES (@Nombre, @Apellido, @NumeroElegido, @Pago)";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Apellido", apellido);
                cmd.Parameters.AddWithValue("@NumeroElegido", numeroElegido);
                cmd.Parameters.AddWithValue("@Pago", pago);

                try
                {
                    cmd.ExecuteNonQuery();
                    lblMessage.Text = "Participante registrado correctamente.";
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627) // Error de duplicación
                    {
                        lblMessage.Text = "El número elegido ya está registrado. Por favor, elija otro número.";
                    }
                    else
                    {
                        lblMessage.Text = $"Error de base de datos: {ex.Message}";
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = $"Error: {ex.Message}";
                }

                CargarParticipantes();
            }
        }

        private void CargarParticipantes()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RifaDBConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT Nombre, Apellido, NumeroElegido, Pago FROM Participantes ORDER BY NumeroElegido ASC";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    lblMessage.Text = $"Error al cargar los participantes: {ex.Message}";
                }

                GridViewParticipantes.DataSource = dt;
                GridViewParticipantes.DataBind();
            }
        }

        protected void GridViewParticipantes_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewParticipantes.EditIndex = e.NewEditIndex;
            CargarParticipantes();
        }

        protected void GridViewParticipantes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewParticipantes.EditIndex = -1;
            CargarParticipantes();
        }

        protected void GridViewParticipantes_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int numeroElegido = (int)GridViewParticipantes.DataKeys[e.RowIndex].Value;
            RadioButtonList rblPago = (RadioButtonList)GridViewParticipantes.Rows[e.RowIndex].FindControl("rblEditPago");
            bool pago = rblPago.SelectedValue == "true";

            string connectionString = ConfigurationManager.ConnectionStrings["RifaDBConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "UPDATE Participantes SET Pago = @Pago WHERE NumeroElegido = @NumeroElegido";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Pago", pago);
                cmd.Parameters.AddWithValue("@NumeroElegido", numeroElegido);

                try
                {
                    cmd.ExecuteNonQuery();
                    lblMessage.Text = "Participante actualizado correctamente.";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = $"Error al actualizar: {ex.Message}";
                }

                GridViewParticipantes.EditIndex = -1;
                CargarParticipantes();
            }
        }

        protected void GridViewParticipantes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int numeroElegido = (int)GridViewParticipantes.DataKeys[rowIndex].Value;

                string connectionString = ConfigurationManager.ConnectionStrings["RifaDBConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "DELETE FROM Participantes WHERE NumeroElegido = @NumeroElegido";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@NumeroElegido", numeroElegido);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        lblMessage.Text = "Participante eliminado correctamente.";
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = $"Error al eliminar participante: {ex.Message}";
                    }

                    CargarParticipantes();
                }
            }
        }


        protected void btnVaciarBaseDatos_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RifaDBConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "DELETE FROM Participantes";
                SqlCommand cmd = new SqlCommand(query, con);

                try
                {
                    cmd.ExecuteNonQuery();
                    lblMessage.Text = "Base de datos vaciada correctamente.";
                    CargarParticipantes();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = $"Error al vaciar la base de datos: {ex.Message}";
                }
            }
        }
    }
}
