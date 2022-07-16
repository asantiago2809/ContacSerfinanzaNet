Imports System.Net
Imports MySql.Data.MySqlClient
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class Contacto
    Inherits System.Web.UI.Page

#Region "Propiedades"
    ' Creamos una propiedad de tipo MySqlConnection, donde almacenaremos, durante la sesion, las credenciales de la base de datos
    Protected Property dbConexion As MySqlConnection
        Get
            Return Session("dbConexion")
        End Get
        Set(value As MySqlConnection)
            Session("dbConexion") = value
        End Set
    End Property
#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Page.Form.Enctype = "multipart/form-data"

            If Not Page.IsPostBack Then
                'Validamos si ya creamos la informarción de la conexión
                If dbConexion Is Nothing Then
                    'En caso de no haberla creado hacemos la instancia con las credenciales de la base de datos
                    dbConexion = New MySqlConnection("Server=MYSQL8002.site4now.net;Database=db_a8a350_prutec;Uid=a8a350_prutec;Pwd=m0BJvXgOQEYxkr!")
                End If

                'Cargamos los paises que vienen desde la API al DropDownList
                CargarPaises()

                'Cargamos los contactos de la base de datos
                CargarContactos()

                'Actualzamos el panel con los datos cargados
                udpnlGeneral.Update()
            End If
        Catch ex As Exception
            'En caso de haber un error, mandamos el mensaje de ese error al metodo que ejecuta el JavaScript para mostrar un Toast con el Mensaje
            SystemMessage("error", ex.Message, "Error")
        End Try
    End Sub

#End Region

#Region "Metodos"

    ' METODO PARA CONSUMIR LA API REST
    Private Sub CargarPaises()
        Try
            'Primero creamos una instancia del WebClient para acceder al URL de la API
            Dim Client As New WebClient

            'Segundo descargamos el JSon en forma de String usando el WebClient
            Dim strPaises As String = Client.DownloadString("http://country.io/names.json")

            'Tercero hacemos la conversion de ese string, mediante el metodo DeserializeObject del JsonConvert, al JObject
            Dim joPaises As JObject = JsonConvert.DeserializeObject(Of Object)(strPaises)

            'Cuarto asignamos la colección de cada item en forma de lista de tipo JToken
            Dim lstPaises As List(Of JToken) = joPaises.Children().ToList

            'Quinto limpiamos los items previos en la ddl y le asignamos los valores de la lista previa (Los cuales serian los nombres de los paises).
            ddlPaisesContacto.Items.Clear()
            ddlPaisesContacto.DataSource = lstPaises.Values
            ddlPaisesContacto.DataBind()
        Catch ex As Exception
            'En caso de haber un error, mandamos el mensaje de ese error al metodo que ejecuta el JavaScript para mostrar un Toast con el Mensaje
            SystemMessage("error", ex.Message, "Error")
        End Try
    End Sub

    ' VARIABLE QUE ENVIAMOS AL FRONT PARA MOSTRAR EL NUMERO DE CONTACTOS
    Protected num_contactos As Integer = 0
    ' METODO PARA CARGAR LOS CONTACTOS
    Private Sub CargarContactos()
        Try
            'Creamos el sql, por defecto vendra ordenado por el ID (En este caso, la tabla la llame "contacto")
            Dim sql = "SELECT * FROM contacto"

            'Abrimos la base de datos
            dbConexion.Open()

            'Creamos el comando y lo ejecutamos, almacenando el resultado en este caso
            Dim dbConexionCommand = New MySqlCommand(sql, dbConexion)
            Dim dbConexionReader = dbConexionCommand.ExecuteReader()

            'Cargamos los datos obtenidos del reader a una datatable
            Dim dtContactos As DataTable = New DataTable
            dtContactos.Load(dbConexionReader)

            'Validamos que efectivamente si existan contactos
            If Not dtContactos.Rows.Count = 0 Then
                'Si hay, mostramos el numero total de contactos y asignamos la datatable a la gridview
                num_contactos = dtContactos.Rows.Count
                gvContactos.DataSource = dtContactos
                gvContactos.DataBind()
            Else
                'Si no hay limpiamos nuestra gridview
                gvContactos.DataSource = Nothing
                gvContactos.DataBind()
            End If

            'Por último cerramos el reader y la conexion a la base de datos
            dbConexionReader.Close()
            dbConexion.Close()
        Catch ex As Exception
            'En caso de haber un error, mandamos el mensaje de ese error al metodo que ejecuta el JavaScript para mostrar un Toast con el Mensaje
            SystemMessage("error", ex.Message, "Error")
        End Try
    End Sub

    ' METODO PARA MOSTRAR UN MENSAJE (TOAST) SEGUN EL TIPO (SI ES DE ERROR O DE EXITO)
    Private Sub SystemMessage(type As String, mensaje As String, titulo As String)
        Dim Script = "toastr['" & type & "']('" & mensaje & "', '" & titulo & "')"

        Dim Rnd As New Random
        Dim rand = Rnd.Next(1990)
        Dim key = "JavaScript" + rand.ToString()
        Dim Jscript = "<script type='text/javascript'>" + Script + "</script>"

        ScriptManager.RegisterStartupScript(Me, Page.GetType, key, Jscript, False)
    End Sub

    ' METODO PARA LIMPIAR EL FORM DE DATOS
    Private Sub LimpiarForm()
        txtNombreContacto.Text = ""
        ddlPaisesContacto.SelectedIndex = 0
        txtCelularContacto.Text = ""
        txtCorreoContacto.Text = ""
    End Sub
#End Region

#Region "Eventos"
    ' Evento del boton para registrar un contacto
    Private Sub btnRegistrarContacto_Click(sender As Object, e As EventArgs) Handles btnRegistrarContacto.Click
        Try
            ' Recibimos los valores del front con las validaciones
            Dim nombre = txtNombreContacto.Text.ToString.Trim
            Dim pais = ddlPaisesContacto.SelectedItem.ToString
            Dim celular = Long.Parse(txtCelularContacto.Text.ToString)
            Dim correo = txtCorreoContacto.Text.ToString.Trim

            ' Creamos nuestro sql de insercion en nuestra tabla (En este caso, la tabla la llame "contacto")
            Dim sql = "INSERT INTO contacto(NOMBRE,PAIS,CELULAR,CORREO) VALUES('" & nombre & "','" & pais & "','" & celular & "','" & correo & "')"

            'Abrimos la base de datos
            dbConexion.Open()

            'Creamos el comando y lo ejecutamos, almacenando el resultado en este caso
            Dim dbConexionCommand = New MySqlCommand(sql, dbConexion)
            Dim dbConexionReader = dbConexionCommand.ExecuteReader()

            'Cerramos el reader y la conexion a la base de datos
            dbConexionReader.Close()
            dbConexion.Close()

            ' Mostramos el toast con el mensaje de exito y limpiamos el form
            SystemMessage("success", "Contacto registrado.", "Exito!")
            LimpiarForm()

            ' Por ultimo cargamos los contactos actualizados
            CargarContactos()
        Catch ex As Exception
            'En caso de haber un error, mandamos el mensaje de ese error al metodo que ejecuta el JavaScript para mostrar un Toast con el Mensaje
            SystemMessage("error", ex.Message, "Error")
        End Try
    End Sub
#End Region

End Class