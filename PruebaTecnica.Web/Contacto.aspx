<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Contacto.aspx.vb" Inherits="PruebaTecnica.Web.Contacto" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Content/toastr.min.css" />
    <div class="row" style="margin-top:1em;">
        <div class="col-xs-12">
            <div class="card">
                <div class="card-body">
                    <%-- INICIO UPDATE PANEL GENERAL --%>
                    <asp:UpdatePanel runat="server" ID="udpnlGeneral" class="height100" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="componentHeight">
                                <%-- INICIO FILA REGISTRO CONTACTO --%>
                                <div class="staticHeight">
                                    <div class="cajafiltros">
                                        <div class="row">

                                            <%-- NOMBRE --%>
                                            <div class="form-group col-md-2 col-lg-2 col-sm-6 col-xs-12">
                                                <asp:Label runat="server" Text="Nombre Completo:" CssClass="" />
                                                <asp:TextBox runat="server" ID="txtNombreContacto" CssClass="form-control input-xs" Style="width:100%;"/>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvtxtNombre" ControlToValidate="txtNombreContacto"
                                                    ErrorMessage=" Campo obligatorio " ForeColor="Red" ValidationGroup="DatosDelContacto" />
                                            </div>

                                            <%-- PAIS --%>
                                            <div class="form-group col-md-2 col-lg-2 col-sm-6 col-xs-12">
                                                <asp:Label runat="server" Text="País:" CssClass="" />
                                                <asp:DropDownList CssClass="form-control input-xs" ID="ddlPaisesContacto" AutoPostBack="true" runat="server" />
                                            </div>

                                            <%-- CELULAR --%>
                                            <div class="form-group col-md-2 col-lg-2 col-sm-6 col-xs-12">
                                                <asp:Label runat="server" Text="Celular:" CssClass="" />
                                                <asp:TextBox runat="server" ID="txtCelularContacto" CssClass="form-control input-xs" MaxLength="10" onkeypress="return ValidacionSoloNumeros(event);"/>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvtxtCelularContacto" ControlToValidate="txtCelularContacto"
                                                    ErrorMessage=" Campo obligatorio " ForeColor="Red" ValidationGroup="DatosDelContacto" />
                                            </div>

                                            <%-- CORREO --%>
                                            <div class="form-group col-md-2 col-lg-2 col-sm-6 col-xs-12">
                                                <asp:Label runat="server" Text="Correo:" CssClass="" />
                                                <asp:TextBox runat="server" ID="txtCorreoContacto" CssClass="form-control input-xs" TextMode="Email" />
                                                <asp:RequiredFieldValidator runat="server" ID="rfvtxtCorreoContacto" ControlToValidate="txtCorreoContacto"
                                                    ErrorMessage=" Campo obligatorio " ForeColor="Red" ValidationGroup="DatosDelContacto" />
                                            </div>

                                            <%-- BTN REGISTRAR CONTACTO --%>
                                            <div class="form-group col-md-4 col-lg-2 col-sm-8 col-xs-12">
                                                <asp:Label runat="server" ID="etLabel">&nbsp;</asp:Label>
                                                <asp:Button runat="server" ID="btnRegistrarContacto" Text="Registrar Contacto"
                                                    CssClass="btn btn-primary btn-Small form-control" Style="font-size: 0.8em;" ValidationGroup="DatosDelContacto"/>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%-- FIN FILA REGISTRO CONTACTO --%>

                                <hr />

                                <%-- INICIO GRIDVIEW CONTACTOS REGISTRADOS --%>
                                <asp:UpdatePanel ID="upGvContactos" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                    <ContentTemplate>
                                        <div class="dinamicHeight tablediv ">
                                            <asp:Label runat="server" ID="Label1" Style="font-size:1.5em;"><strong>LISTA DE CONTACTOS: <%=num_contactos%> </strong></asp:Label>
                                            <asp:GridView ID="gvContactos"
                                                runat="server"
                                                CssClass="table  table-hover table-bordered textMayuscularow table-striped "
                                                Style="margin-top:1em;"
                                                AutoGenerateColumns="False"
                                                Width="100%"
                                                DataKeyNames=""
                                                GridLines="None"
                                                ShowHeaderWhenEmpty="False">
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="#" />
                                                    <asp:BoundField DataField="NOMBRE" HeaderText="NOMBRE" />
                                                    <asp:BoundField DataField="PAIS" HeaderText="PAÍS" />
                                                    <asp:BoundField DataField="CELULAR" HeaderText="CELULAR" />
                                                    <asp:BoundField DataField="CORREO" HeaderText="CORREO" />
                                                </Columns>
                                                <PagerStyle CssClass="pagination-ys" />
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>
                                <%-- FIN GRIDVIEW CONTACTOS REGISTRADOS --%>

                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnRegistrarContacto" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <%-- FIN UPDATE PANEL GENERAL --%>
                </div>
            </div>
        </div>
    </div>

    <%-- INICIO METODOS JAVASCRIPT --%>
    <script src="Scripts/toastr.min.js"></script>
    <script>
        function ValidacionSoloNumeros(evt) {
            //Se asigna el valor de la tecla a keynum
			if (window.event) {
				keynum = evt.keyCode;
			}else {
				keynum = evt.which;
            }

			//Se valida que si se encuentra en el rango numérico y que teclas no recibirá.
			if ((keynum > 47 && keynum < 58) || keynum == 8 || keynum == 13 || keynum == 6) {
				return true;
			}else {
				return false;
			}
        }
    </script>
    <%-- FIN METODOS JAVASCRIPT --%>

</asp:Content>