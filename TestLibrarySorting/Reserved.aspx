<%@ Page Title="My Reserved Books" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Reserved.aspx.cs" Inherits="SeminaryLibrary.ReservedBooks" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        My Reserved Books
    </h2>
    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
    AllowSorting="True" DataKeyNames="date,BookId" ShowHeaderWhenEmpty="True" 
        EnablePersistedSelection="True" onsorting="GridView1_Sorting" 
        onpageindexchanging="GridView1_PageIndexChanging" BackColor="White" 
        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"  Width="100%" OnDataBound="GridView1_DataBound" >
        <FooterStyle BackColor="White" ForeColor="#000066" />
        <HeaderStyle BackColor="#469DC7" Font-Bold="True" ForeColor="White"  />
        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
        <RowStyle ForeColor="#000066" Wrap="true"/>
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#007DBB" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#00547E" />
    </asp:GridView>
</asp:Content>
