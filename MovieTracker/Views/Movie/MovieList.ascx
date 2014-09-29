<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.IEnumerable<MovieTracker.Data.Movie>>" %>           
<% if(Model.Count() == 0)
               { %>
                <tr class="movieDetailRow"><td colspan="4" id="noResults"> No Results Found</td></tr>
             <%} %>
            <% foreach (var movie in Model)
               {
            %>
<tr class="movieDetailRow">
                <td>
                    <%if(movie.BorrowerName != null) {%>
                    <img src="../../Content/not_available.png" id="lentOutStatus<%:movie.Id %>" data-movieid="<%:movie.Id %>" height="20" width="20"/>
                    <%}else{ %>
                    <img src="../../Content/available.png" id="lentOutStatus<%:movie.Id %>" data-movieid="<%:movie.Id %>" height="20" width="20"/>
                    <%} %>
                </td>
                <td>
                    <a href="<%:Url.Action("Edit", new { id = movie.Id }) %>" class="edit" title="Edit <%:movie.Name %>"></a>
                </td>
                <td>
                    <%:Html.ActionLink(movie.Name, "Detail", new {id=movie.Id}) %>
                    <div id="lentOutDetails<%:movie.Id %>">
                        <%if(movie.BorrowerName != null){ %>
                         <%: Html.Partial("MovieListLentStatus", movie) %>
                        <%} %>
                    </div>
                </td>
                <td>
                    <%: Html.Partial("MovieRatingControl", movie) %>
                </td>
                <td>
                    <a href="<%:Url.Action("Delete", new { id = movie.Id }) %>" class="delete" title="Delete <%:movie.Name %>"></a>
                </td></tr>
            <%
               } %>

