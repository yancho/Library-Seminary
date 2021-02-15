<%@ Page Title="LogOut" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Logout.aspx.cs" Inherits="SeminaryLibrary.Account.LogOut" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Log Out
    </h2>
    <p>
        <asp:Literal ID="LogOutMsg" runat="server"></asp:Literal>
    </p>
    
</asp:Content>
