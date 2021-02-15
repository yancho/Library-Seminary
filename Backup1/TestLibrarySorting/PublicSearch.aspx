<%@ Page Title="Search Our Online Catalogue" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="PublicSearch.aspx.cs" Inherits="SeminaryLibrary._PublicDefault"  ValidateRequest="false"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <h1>Search our books</h1>
    <div><p>Enter the name or isbn or keywords: 
    <asp:TextBox ID="txtSearch" runat="server" style="width: 128px" />
<asp:ImageButton ID="btnSearch" ImageUrl="images/searchbutton.jpg"
runat="server" onclick="btnSearch_Click" Height="24px" 
            style="margin-right: 0px; margin-top: 0px" Width="24px" />
<asp:ImageButton ID="btnClear" ImageUrl="images/clearbutton.jpg"
runat="server" Height="24px" onclick="btnClear_Click" Width="24px" />
        </p></div>
    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
    AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="itemfield0" 
    
        EnablePersistedSelection="True"  
        onpageindexchanging="GridView1_PageIndexChanging" onsorting="GridView1_Sorting" 
        PageSize="10" onselectedindexchanged="GridView1_SelectedIndexChanged">
        <FooterStyle BackColor="White" ForeColor="#000066" />
        <HeaderStyle BackColor="#469DC7" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
        <RowStyle ForeColor="#000066" />
        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#007DBB" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#00547E" />
    <Columns>
        <asp:BoundField DataField="itemfield0" HeaderText="Seminary Id" ReadOnly="True" 
            SortExpression="itemfield0" >
        <HeaderStyle Width="5em" />
        </asp:BoundField>

        <asp:BoundField DataField="itemfield19" HeaderText="Class Mark" ReadOnly="True" 
            SortExpression="itemfield19" >
        <HeaderStyle Width="8em" />
        </asp:BoundField>

        <asp:BoundField DataField="itemfield1" HeaderText="Title" 
            SortExpression="itemfield1" ReadOnly="True" >
        <HeaderStyle Width="15em" />
        </asp:BoundField>
        <asp:BoundField DataField="itemfield2" HeaderText="Author" 
            SortExpression="itemfield2" ReadOnly="True" >
        <HeaderStyle Width="15em" />
        </asp:BoundField>
        <asp:BoundField DataField="itemfield3" HeaderText="Publisher" 
            SortExpression="itemfield3" ReadOnly="True" >
        <HeaderStyle Width="10em" />
        </asp:BoundField>
        <asp:BoundField DataField="itemfield20" HeaderText="ISBN" 
            SortExpression="itemfield20" ReadOnly="True" >
        <HeaderStyle Width="5em" />
        </asp:BoundField>
        <asp:BoundField DataField="status" HeaderText="Book Status" 
             ReadOnly="True"  HtmlEncode="false" ItemStyle-HorizontalAlign="Center">
        <HeaderStyle Width="0.5em" />
        </asp:BoundField>
        
    </Columns>
</asp:GridView>
<asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>

</asp:Content>
