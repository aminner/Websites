<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MovieTracker.Data.Movie>" %>
<script type="text/javascript" src="../../Scripts/_MovieLent.js" > </script>
<table>
    <tr>
        <td class="lentLabel">
            <%:Html.Label("Lent To:") %>
        </td>
            <td class="lentInput">
            <%:Html.TextBox("lentTo", Model.BorrowerName, new {id="lentToInput" } )%>
                <%if(Model.BorrowerName != null){ %>
                <img src="../../Content/returned.png" class="movieReturned" onClick="MovieReturned(this)" title="Returned from friend"  data-reloadLoc="MovieDetailLentStatus" data-movieid="<%:Model.Id %>" height="15" width="15"/>
                <%} %>
        </td>
        <td class="lentValidationLabel">
            <span id="lentToValidation"></span>
        </t>
    </tr><tr>
            <td >
            <%:Html.Label("On:") %>
        </td>
            <td>
            <%:Html.TextBox("lentOn", Model.BorrowedDate!=null?Model.BorrowedDate.Value.Date.ToShortDateString():"", new {id="datepicker" } )%>
        </td>
        <td>
            <span id="lentOnValidation"></span>
        </td>
        </tr>
    <tr>
        <td></td>
        <td>
            <input type="button" value="Lent to a friend" id="lentOut" onClick="MovieLentOut()" data-movieId ="<%:Model.Id %>" /></td>
        <td></td>
        </tr>
</table>
