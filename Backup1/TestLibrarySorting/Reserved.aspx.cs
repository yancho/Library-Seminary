using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using SeminaryLibrary.Libraries;
using MySql.Data.MySqlClient;

namespace SeminaryLibrary
{
    public partial class ReservedBooks : System.Web.UI.Page
    {
        SemCurrentUser cu;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState["sortColumn"] = " ";
                ViewState["sortDirection"] = " ";
                ViewState["orderBy"] = " ";
            }

            string userEmail = User.Identity.Name;

           cu  = SemGetCurrentUser.GetCurrentUser(userEmail);

           FillGrid(String.Empty);


        }



        private void FillGrid (string orderby)
        {
            DataSet dataSet = new DataSet();

            

            using (MySqlConnection connection = Connection.GetDBConnection())
            {
                string sql = "SELECT   date as Date    ,itemid  as BookId         ,i.itemfield1 as 'Book Title',i.itemfield2 as 'Book Author',i.itemfield3 as 'Book Publisher',i.itemfield19 as Class,i.itemfield20 as Isbn FROM reservelist r  inner join  users y  on r.userid = y.userfield0  inner join  items i  on r.itemid = i.itemfield0 WHERE   y.userfield18 = ?email      " + orderby;
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.Add("?email", MySqlDbType.VarChar).Value = cu.Email;
                
                command.CommandType = CommandType.Text;

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);

                dataAdapter.Fill(dataSet);
            }

            

            GridView1.DataSource = dataSet.Tables[0];
            GridView1.DataBind();

        

        }


        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["sortColumn"].ToString() == e.SortExpression.ToString())
            {
                if ("ASC" == ViewState["sortDirection"].ToString())
                {
                    ViewState["sortDirection"] = "DESC";
                    string orderBy = String.Format(" ORDER BY `{0}` {1} ",e.SortExpression,ViewState["sortDirection"]);
                    ViewState["orderBy"] = orderBy;
                    FillGrid(orderBy);
                }
                else
                {
                    ViewState["sortDirection"] = "ASC";
                    string orderBy = String.Format(" ORDER BY `{0}` {1} ", e.SortExpression, ViewState["sortDirection"]);
                    ViewState["orderBy"] = orderBy;
                    FillGrid(orderBy);
                }
            }
            else
            {
                ViewState["sortColumn"] = e.SortExpression.ToString();
                ViewState["sortDirection"] = "ASC";
                string orderBy = String.Format(" ORDER BY `{0}` {1} ", e.SortExpression, ViewState["sortDirection"]);
                ViewState["orderBy"] = orderBy;
                FillGrid(orderBy);
            }

            
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            FillGrid(ViewState["orderBy"].ToString());
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            if (GridView1.HeaderRow.Cells.Count > 0)
            {
                GridView1.HeaderRow.Cells[0].Width = Unit.Percentage(5.00);
                GridView1.HeaderRow.Cells[1].Width = Unit.Percentage(5.00);
                GridView1.HeaderRow.Cells[2].Width = Unit.Percentage(20.00);
                GridView1.HeaderRow.Cells[3].Width = Unit.Percentage(25.00);
                GridView1.HeaderRow.Cells[4].Width = Unit.Percentage(10.00);
                GridView1.HeaderRow.Cells[5].Width = Unit.Percentage(15.00);
                GridView1.HeaderRow.Cells[6].Width = Unit.Percentage(15.00);

            }
        }
    }
}
