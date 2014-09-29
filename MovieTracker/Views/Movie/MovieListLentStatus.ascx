<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MovieTracker.Data.Movie>" %>
    <%if(Model.BorrowerName != null) { %>
        <script type="text/javascript" src="../../Scripts/_MovieLent.js" ></script>
                 <%:Html.Label("Lent To: " + Model.BorrowerName) %>
               <img src="../../Content/returned.png" class="movieReturned" onclick="MovieReturned(this)"  title="Returned from friend"  data-reloadLoc="MovieListLentStatus" data-movieid="<%:Model.Id %>" height="15" width="15"/>
              <br /> <%: Html.Label(" On: " + Model.BorrowedDate.Value.Date.ToShortDateString()) %>
      <%}%>