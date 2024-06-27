<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RifaSistem.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registro de Participantes para la Rifa</title>
    <link rel="stylesheet" href="styles.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Registro de Participantes para la Rifa</h2>
            
            <div>
                <asp:Label ID="lblNombre" runat="server" Text="Nombre:"></asp:Label>
                <br />
                <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>
            </div>
            <div>
                <asp:Label ID="lblApellido" runat="server" Text="Apellido:"></asp:Label>
                <br />
                <asp:TextBox ID="txtApellido" runat="server"></asp:TextBox>
            </div>
            <div>
                <asp:Label ID="lblNumeroElegido" runat="server" Text="Número Elegido:"></asp:Label>
                <br />
                <asp:TextBox ID="txtNumeroElegido" runat="server"></asp:TextBox>
            </div>

            <div class="containerR">
                
                <div class="radio-group">
                    <asp:Label ID="lblPago" runat="server" Text="¿Pagó?:"></asp:Label>
                <asp:RadioButtonList ID="rblPago" runat="server">
                    <asp:ListItem Text="Sí" Value="true"></asp:ListItem>
                    <asp:ListItem Text="No" Value="false"></asp:ListItem>
                </asp:RadioButtonList>
                    </div>
            </div>
            <div>
                <asp:Button ID="btnInsertar" runat="server" Text="Insertar" CssClass="btn" OnClick="btnInsertar_Click" />
                <asp:Button ID="btnVaciarBaseDatos" runat="server" Text="Vaciar Base de Datos" CssClass="btn" OnClick="btnVaciarBaseDatos_Click" />
            </div>
            <div>
                <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>
            
            <h2>Participantes Registrados</h2>
            <div class="grid-container">
                <asp:GridView ID="GridViewParticipantes" runat="server" AutoGenerateColumns="False" DataKeyNames="NumeroElegido"
                    OnRowEditing="GridViewParticipantes_RowEditing" OnRowCancelingEdit="GridViewParticipantes_RowCancelingEdit"
                    OnRowUpdating="GridViewParticipantes_RowUpdating" OnRowCommand="GridViewParticipantes_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" ReadOnly="true" />
                        <asp:BoundField DataField="Apellido" HeaderText="Apellido" ReadOnly="true" />
                        <asp:BoundField DataField="NumeroElegido" HeaderText="Número Elegido" ReadOnly="true" />
                        <asp:TemplateField HeaderText="¿Pagó?">
                            <ItemTemplate>
                                <%# (bool)Eval("Pago") ? "Sí" : "No" %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:RadioButtonList ID="rblEditPago" runat="server">
                                    <asp:ListItem Text="Sí" Value="true"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                </asp:RadioButtonList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton CssClass="btnEliminar" runat="server" CommandName="Eliminar" Text="Eliminar" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('¿Estás seguro que deseas eliminar este participante?');" />
                        </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
