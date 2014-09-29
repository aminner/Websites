<%@ Page Title="Title" Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<MovieTracker.Data.Movie>>"
    MasterPageFile="~/Views/Shared/Site.Master" EnableViewState="false" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">
    My Movies</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
 <script type="text/javascript" src="../../Scripts/_Filter.js"></script>
     
    <table class="movieList">
        <thead>
            <tr>
                <td colspan="5">    <%:Html.TextBox("search", "", new {placeholder="Search", id="filterList"}) %></td>
            </tr>
            <tr><th>
                    Available
                </th>
                <th>
                    Edit
                </th>
                <th>
                    Name
                </th>
                <th>
                    Rating
                </th>
                <th>
                    Delete
                </th>
            </tr>
        </thead>
        <tbody>
            <%: Html.Partial("MovieList", Model) %>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="5">
                    <%:Html.ActionLink("Add new movie", "Add") %>
                </td>
            </tr>
        </tfoot>
    </table>
</asp:Content>
