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


    public partial class _PublicDefault : System.Web.UI.Page
    {


        private void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                ViewState["sortOrder"] = "asc";
                ViewState["semId"] = "";
                ViewState["classmark"] = "";
                ViewState["title"] = "";
                ViewState["author"] = "";
                ViewState["publisher"] = "";
                ViewState["isbn"] = "";
                ViewState["search"] = "";

                ViewState["selected"] = new ArrayList();

                GetBooksDependingOnField(ViewState["semId"].ToString(), ViewState["classmark"].ToString(), ViewState["title"].ToString(), ViewState["author"].ToString(), ViewState["publisher"].ToString(), ViewState["isbn"].ToString(), "", "");
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
                ViewState["classmark"] = txtSearch.Text;
                ViewState["title"] = txtSearch.Text;
                ViewState["author"] = txtSearch.Text;
                ViewState["publisher"] = txtSearch.Text;
                ViewState["isbn"] = txtSearch.Text;
                GridView1.DataSource = null;
                Label1.Text = "";

                GridView1.DataSourceID = String.Empty;
                GridView1.Dispose();


                GetBooksDependingOnField(ViewState["semId"].ToString(), ViewState["classmark"].ToString(), ViewState["title"].ToString(), ViewState["author"].ToString(), ViewState["publisher"].ToString(), ViewState["isbn"].ToString(), "", "asc");


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

        public void GetBooksDependingOnField(string semid, string classmark, string title, string author, string publisher, string isbn, string sortExp, string sortDir)
        {
            DataSet dataSet = new DataSet();


            using (MySqlConnection connection = Connection.GetDBConnection())
            {
                string sql = "SELECT  itemfield0, itemfield19, itemfield1, itemfield2, itemfield3, itemfield20,  case   when itemfield7 = 'OUT' THEN '<img src=\"Images/borrowed.png\" alt=\"Book Is Borrowed\" title=\"Book is Borrowed\" />'     WHEN itemfield7 = 'NEW' and reservelist.itemid is NOT null THEN '<img src=\"Images/reserved.png\" alt=\"Book Is Reserved\" title=\"Book is Reserved\" />'  when itemfield7 = 'NEW' and reservelist.itemid is null THEN '<img src=\"Images/new.png\" alt=\"Book Is New\" title=\"Book is New\" />' WHEN itemfield7 = 'IN' and reservelist.itemid is NOT null THEN '<img src=\"Images/reserved.png\" alt=\"Book Is Reserved\" title=\"Book is Reserved\" />'   when itemfield7 = 'IN' and reservelist.itemid is null THEN '<img src=\"Images/ok.png\" alt=\"Book Is Available\" title=\"Book is Available\" />'    END as status  FROM items LEFT JOIN  reservelist ON itemfield0 = reservelist.itemid WHERE   itemfield0 LIKE ?semid  OR itemfield19 LIKE ?classmark OR itemfield1 LIKE ?title OR itemfield2 like ?author OR itemfield3 LIKE ?publisher OR itemfield20 LIKE ?isbn";
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.Add("?semid", MySqlDbType.VarChar).Value = semid;
                command.Parameters.Add("?classmark", MySqlDbType.VarChar).Value = "%" + classmark + "%";
                command.Parameters.Add("?title", MySqlDbType.VarChar).Value = "%" + title + "%";
                command.Parameters.Add("?author", MySqlDbType.VarChar).Value = "%" + author + "%";
                command.Parameters.Add("?publisher", MySqlDbType.VarChar).Value = "%" + publisher + "%";
                command.Parameters.Add("?isbn", MySqlDbType.VarChar).Value = "%" + isbn + "%";
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

            GetBooksDependingOnField(ViewState["semId"].ToString(), ViewState["classmark"].ToString(), ViewState["title"].ToString(), ViewState["author"].ToString(), ViewState["publisher"].ToString(), ViewState["isbn"].ToString(), e.SortExpression, sortOrder);
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



     




    }
}
