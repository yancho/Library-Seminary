<%@ Page Title="Search And Reserve Books From Our Online Catalogue" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Search.aspx.cs" Inherits="SeminaryLibrary._Default"  ValidateRequest="false"%>

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
        <asp:TemplateField HeaderText="Select"  >
            <ItemTemplate>
                <asp:CheckBox runat="server" ID="chkChecked" Enabled="true"  AutoPostBack="false" Visible="true"
                 />
            </ItemTemplate>
            <HeaderStyle Width="0.5em" />
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:Button ID="CheckAll" runat="server" Text="Check All" 
        onclick="CheckAll_Click" />
 
<asp:Button ID="UncheckAll" runat="server" Text="Uncheck All" 
        onclick="UncheckAll_Click" />

    <asp:Button ID="btnBookBooks" runat="server" onclick="Button1_Click" 
        Text="Mark the Selected Books as Favourite" />

<asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>

    <br />
    <br />

    <asp:Label ID="FavouriteBksLbl" runat="server" Text="" ForeColor="ActiveCaption"></asp:Label>
    <asp:CheckBoxList ID="SelectedCBList" runat="server" 
        onselectedindexchanged="SelectedCBList_SelectedIndexChanged" AutoPostBack="true">
    </asp:CheckBoxList>
    <asp:Button ID="btnReserveBooks" runat="server" 
        Text="" Enabled="false" 
        onclick="btnReserveBooks_Click" />

    <asp:Label ID="errMaxBooks" runat="server" Visible="False" Font-Bold="True" ForeColor="Red" 
        Font-Italic="True"></asp:Label>

    <asp:Label ID="confirmBookingLbl" runat="server" ForeColor="#33CC33"></asp:Label>

</asp:Content>
