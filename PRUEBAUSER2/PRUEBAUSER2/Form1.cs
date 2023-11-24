using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.Logging;
using System;
using System.ComponentModel;
using System.Data;

namespace PRUEBAUSER2
{
    public partial class Form1 : Form
    {
        
        
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {

            // Configura la cadena de conexi�n con autenticaci�n de SQL Server
            string adminConnectionString = "Data Source=DESKTOP-0LIQUQM\\SQLEXPRESS;Initial Catalog=pruebaUSER1;User Id=alex2;Password=Alejandro2023*;TrustServerCertificate=True;";

            // Obtiene el nombre del usuario y su contrase�a desde los controles de Windows Forms
            string nuevoUsuario = txtUsuario.Text;
            string nuevaContrase�a = txtContrase�a.Text;

            // Crea la conexi�n a la base de datos como administrador
            using (SqlConnection adminConnection = new SqlConnection(adminConnectionString))
            {
                // Abre la conexi�n
                adminConnection.Open();

                // Crea un comando para ejecutar la creaci�n del nuevo usuario
                using (SqlCommand createUserCommand = new SqlCommand($"CREATE LOGIN {nuevoUsuario} WITH PASSWORD = '{nuevaContrase�a}', DEFAULT_DATABASE = [pruebaUSER1];", adminConnection))
                {
                    
                        // Ejecuta el comando para crear el nuevo usuario
                        createUserCommand.ExecuteNonQuery();
                        MessageBox.Show("Usuario creado con �xito.");
                        try {
                            // Ahora, conecta como el nuevo usuario para ejecutar comandos en su contexto
                            string userConnectionString = $"Data Source=DESKTOP-0LIQUQM\\SQLEXPRESS;Initial Catalog=pruebaUSER1;User Id={nuevoUsuario};Password={nuevaContrase�a};TrustServerCertificate=True;";
                            using (SqlConnection userConnection = new SqlConnection(userConnectionString))
                            {
                                // Abre la conexi�n como el nuevo usuario
                                userConnection.Open();
                                // Crea un comando para insertar el nuevo usuario en la tabla 'usuarios'
                                using (SqlCommand insertCommand = new SqlCommand("INSERT INTO usuarios (nombreUsuario, contrase�a) VALUES (@NombreUsuario, @Contrase�a)", userConnection))
                                {
                                    // Agrega par�metros al comando
                                    insertCommand.Parameters.AddWithValue("@NombreUsuario", nuevoUsuario);
                                    insertCommand.Parameters.AddWithValue("@Contrase�a", nuevaContrase�a);

                                    // Ejecuta el comando para insertar el nuevo usuario
                                    insertCommand.ExecuteNonQuery();
                                    MessageBox.Show("Usuario insertado con �xito en la tabla 'usuarios'.");

                                  //Crea un comando para otorgar privilegios al nuevo usuario
                                   using (SqlCommand grantPermissionsCommand = new SqlCommand(
                                   $"USE pruebaUSER1; " +
                                   $"CREATE USER {nuevoUsuario} FOR LOGIN {nuevoUsuario}; " +
                                   $"ALTER ROLE db_datareader ADD MEMBER {nuevoUsuario}; " +
                                   $"ALTER ROLE db_datawriter ADD MEMBER {nuevoUsuario};" +
                                   $"ALTER ROLE db_owner ADD MEMBER {nuevoUsuario};", userConnection))
                                   {

                                    // Ejecuta el comando para otorgar privilegios al nuevo usuario
                                    grantPermissionsCommand.ExecuteNonQuery();
                                    MessageBox.Show("Privilegios otorgados al usuario en la base de datos.");
                                   }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al iniciar seccion con el usuario nuevo: " + ex.Message);
                        }
                    
                }
            }



        }
    }
}