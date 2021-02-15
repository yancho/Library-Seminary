using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;
using System.Configuration;
using SeminaryLibrary.Libraries;
using MySql.Data.MySqlClient;

namespace SeminaryLibrary
{

    
    public partial class _Default : System.Web.UI.Page
    {

        int maxBooks = Convert.ToInt32(ConfigurationManager.AppSettings["maxBooks"]);
        int maxBookingPeriod = Convert.ToInt32(ConfigurationManager.AppSettings["maxBookingPeriod"]);
        
        protected void Page_Load(object sender, EventArgs e)
        {

           
            if (!IsPostBack)
            {
                ViewState["sortOrder"] = "asc";
                ViewState["semId"]      = "";
                ViewState["title"]      = "";
                ViewState["author"]     = "";
                ViewState["publisher"]  = "";
                ViewState["isbn"]       = "";
                ViewState["classmk"] = "";
                ViewState["search"] = "";

                ViewState["selected"] = new ArrayList();

                GetBooksDependingOnField(ViewState["semId"].ToString(), ViewState["title"].ToString(), ViewState["author"].ToString(), ViewState["publisher"].ToString(), ViewState["isbn"].ToString(), ViewState["classmk"].ToString(), "", "");
            }
        }



        protected string search = String.Empty;

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {

            search = txtSearch.Text;
            ViewState["search"] = search;
            GetBooks();
            
        }

        private void GetBooks()
        {
            try
            {
                ViewState["sortOrder"] = "asc";
                ViewState["semId"] = txtSearch.Text;
                ViewState["title"] = txtSearch.Text;
                ViewState["author"] = txtSearch.Text;
                ViewState["publisher"] = txtSearch.Text;
                ViewState["isbn"] = txtSearch.Text;
                ViewState["classmk"] = txtSearch.Text;
                GridView1.DataSource = null;
                Label1.Text = "";

                GridView1.DataSourceID = String.Empty;
                GridView1.Dispose();


                GetBooksDependingOnField(ViewState["semId"].ToString(), ViewState["title"].ToString(), ViewState["author"].ToString(), ViewState["publisher"].ToString(), ViewState["isbn"].ToString(), ViewState["classmk"].ToString(), "", "asc");
                

                if (GridView1.Rows.Count < 1)
                {
                    Label1.Text = "<h2> No books match your query. Please try again</h2>";

                }
            }
            catch (SqlException ex)
            {

                Label1.Text = "Cannot get product data." + ex.Message;
            }
        }

        public void GetBooksDependingOnField(string semid, string title, string author, string publisher, string isbn, string classmk, string sortExp, string sortDir)
        {
            DataSet dataSet = new DataSet();


            using (MySqlConnection connection = Connection.GetDBConnection())
            {
                string sql = "SELECT  itemfield0, itemfield1, itemfield2, itemfield3, itemfield20, itemfield19 FROM items WHERE   itemfield0 NOT IN (SELECT itemid FROM reservelist) AND                itemfield0 LIKE ?semid OR itemfield1 LIKE ?title OR itemfield2 like ?author OR itemfield3 LIKE ?publisher OR itemfield20 LIKE ?isbn OR itemfield19 LIKE ?classmk";
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.Add("?semid", MySqlDbType.VarChar).Value = semid;
                command.Parameters.Add("?title", MySqlDbType.VarChar).Value = "%" + title + "%";
                command.Parameters.Add("?author", MySqlDbType.VarChar).Value = "%" + author + "%";
                command.Parameters.Add("?publisher", MySqlDbType.VarChar).Value = "%" + publisher + "%";
                command.Parameters.Add("?isbn", MySqlDbType.VarChar).Value = "%" + isbn + "%";
                command.Parameters.Add("?classmk", MySqlDbType.VarChar).Value = "%" + classmk + "%";
                command.CommandType = CommandType.Text;

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                
                dataAdapter.Fill(dataSet);
            }

            DataView myDataView = new DataView();
            myDataView = dataSet.Tables[0].DefaultView;

            if (sortExp != string.Empty)
            {
                myDataView.Sort = string.Format("{0} {1}", sortExp, sortDir);
            }
            
            

            GridView1.DataSource = null;
            Label1.Text = "";

            GridView1.DataSourceID = String.Empty;
            GridView1.Dispose();

            GridView1.DataSource = myDataView;
            GridView1.DataBind();

            if (ViewState["search"] != String.Empty)
            {
                foreach (GridViewRow oItem in GridView1.Rows)
                {
                    for (int i = 0; i < oItem.Cells.Count - 1; i++)
                    {
                        oItem.Cells[i].Text = FoundText(ViewState["search"].ToString(), oItem.Cells[i].Text);
                    }
                }
            }
            

        }

        public string sortOrder
        {
            get
            {
                if (ViewState["sortOrder"].ToString() == "asc")
                {
                    ViewState["sortOrder"] = "desc";
                }
                else
                {
                    ViewState["sortOrder"] = "asc";
                }

                return ViewState["sortOrder"].ToString();
            }
            set
            {
                ViewState["sortOrder"] = value;
            }
        }

        protected void btnClear_Click(object sender, ImageClickEventArgs e)
        {
            txtSearch.Text = "";
            Label1.Text = "";
            ViewState["search"] = "";
            GetBooks();

        }



        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GetBooks();
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {

            GetBooksDependingOnField(ViewState["semId"].ToString(), ViewState["title"].ToString(), ViewState["author"].ToString(), ViewState["publisher"].ToString(), ViewState["isbn"].ToString(), ViewState["classmk"].ToString(), e.SortExpression, sortOrder);
        }


        protected string FoundText(string searchitem, string input)
        {

            Regex myexpr = new Regex(searchitem.Replace(" ", "|"),
            RegexOptions.IgnoreCase);

            return myexpr.Replace(input, new MatchEvaluator(Replacewords));

        }



        public string Replacewords(Match word)
        {

            return "<span class='found'>" + word.Value + "</span>";

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        void ToggleCheckState(bool checkState)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                // Access the CheckBox
                CheckBox cb = (CheckBox)row.FindControl("chkChecked");
                if (cb != null)
                    cb.Checked = checkState;
            }
        }

 

        protected void Button1_Click(object sender, EventArgs e)
        {
            bool atLeastOneRowSelected = false;
            // Iterate through the Products.Rows property
            foreach (GridViewRow row in GridView1.Rows)
            {
                // Access the CheckBox
                CheckBox cb = (CheckBox)row.FindControl("chkChecked");
                if (cb != null && cb.Checked)
                {
                    atLeastOneRowSelected = true;

                    string bookId =
                        Convert.ToString(GridView1.DataKeys[row.RowIndex].Value);

                    string bookTitle = row.Cells[1].Text;
                    string bookAuthor = row.Cells[2].Text;
                    string bookISBN = row.Cells[4].Text;


                    string bookString = String.Format("{0} - {1} by {2} [{3}]", bookId, bookTitle, bookAuthor, bookISBN);
                    if (!CheckIfItemExists(bookString))
                    {
                        SelectedCBList.Items.Add(bookString);
                        FavouriteBksLbl.Text = "<h2>Books marked as favourite</h2>";
                        
                    }
                }
            }
            // Show the Label if at least one row was deleted...
            btnBookBooks.Visible = atLeastOneRowSelected;
        }

        private bool CheckIfItemExists(string newstring)
        {
            foreach (ListItem li in SelectedCBList.Items)
            {
                if (li.Text == newstring)
                    return true;
            }
            return false;
        }

        protected void CheckAll_Click(object sender, EventArgs e)
        {
            ToggleCheckState(true);
        }

        protected void UncheckAll_Click(object sender, EventArgs e)
        {
            ToggleCheckState(false);
        }

        protected void btnReserveBooks_Click(object sender, EventArgs e)
        {

            if (Libraries.BookItems.CheckSelectedItems(SelectedCBList,maxBooks))
            {

                Libraries.ShowJSAlert.Show("Please note that you can book only " + maxBooks.ToString() + " books. Choose only " + maxBooks.ToString() + " items.");
            }
            else
            {
                int i = Libraries.BookItems.ReserveTheseBooksToTheCurrentUser(SelectedCBList, User.Identity.Name);
                if (i > 0)
                {
                    confirmBookingLbl.Text = String.Format("Your booking has been confirmed for {0} books. Please remember to collect the books before {1}", i, DateTime.Now.AddDays(maxBookingPeriod).ToShortDateString());
                    btnReserveBooks.Enabled = false;
                }
                else
                {
                    confirmBookingLbl.Text = String.Format("No Books Booked! Try Again");
                    btnReserveBooks.Enabled = true;
                }
            }

        }

        protected void SelectedCBList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Libraries.BookItems.CheckIfAnItemIsAtLeastSelected(SelectedCBList))
            {

                btnReserveBooks.Enabled = true;
            }

            if (Libraries.BookItems.CheckSelectedItems(SelectedCBList, maxBooks))
            {

                errMaxBooks.Text = "Please note that you can book only " + maxBooks.ToString() + " books. Choose only " + maxBooks.ToString() + " items.";

                errMaxBooks.Visible = true;
                btnReserveBooks.Enabled = false;
            }
            else
            {
                errMaxBooks.Visible = false;
                btnReserveBooks.Enabled = true;
            }

        }





    }
}
