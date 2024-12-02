using System;
using System.Data;
using System.Web.UI;

namespace prjWebCsAdodatabaset
{
    public partial class webDatasetlocal : System.Web.UI.Page
    {
        // Déclaration d'un objet DataSet global
        public static DataSet setSport;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                setSport = CreerDataset();
                gridJoueurs.DataSource = setSport.Tables["Joueurs"];
                gridJoueurs.DataBind();
                RemplirListeEquipe();
            }
        }

        private void RemplirListeEquipe()
        {
            lstEquipes.DataTextField = "Nom";
            lstEquipes.DataValueField = "RefEquipe"; // Correction ici
            lstEquipes.DataSource = setSport.Tables["Equipes"];
            lstEquipes.DataBind();
        }

        protected void lstEquipes_selectdIndexChanged(object sender, EventArgs e)
        {
            // Vérifie si un élément est sélectionné
            if (lstEquipes.SelectedItem == null)
                return;

            string refEquipeChoisie = lstEquipes.SelectedValue;

            // Parcourt la table "Equipes" pour récupérer les données de l'équipe sélectionnée
            foreach (DataRow myRow in setSport.Tables["Equipes"].Rows)
            {
                if (myRow["RefEquipe"].ToString() == refEquipeChoisie)
                {
                    txtNom.Text = myRow["Nom"].ToString();
                    txtVille.Text = myRow["Ville"].ToString();
                    txtBudget.Text = myRow["Budget"].ToString();
                    txtCoach.Text = myRow["Coach"].ToString();
                    break;
                }
            }

            // Filtre et affiche les joueurs de l'équipe sélectionnée
            DataView joueursView = new DataView(setSport.Tables["Joueurs"])
            {
                RowFilter = $"RefEquipe = {refEquipeChoisie}"
            };
            gridJoueurs.DataSource = joueursView;
            gridJoueurs.DataBind();
        }

        private DataSet CreerDataset()
        {
            DataSet myset = new DataSet();

            // Création de la table "Equipes"
            DataTable myTB = new DataTable("Equipes");

            myTB.Columns.Add(new DataColumn("RefEquipe", typeof(long))
            {
                AutoIncrement = true,
                AutoIncrementSeed = 1,
                AutoIncrementStep = 1
            });
            myTB.Columns.Add(new DataColumn("Nom", typeof(string)) { MaxLength = 50 });
            myTB.Columns.Add(new DataColumn("Ville", typeof(string)) { MaxLength = 50 });
            myTB.Columns.Add(new DataColumn("Budget", typeof(decimal)));
            myTB.Columns.Add(new DataColumn("Coach", typeof(string)) { MaxLength = 50 });
            myTB.PrimaryKey = new[] { myTB.Columns["RefEquipe"] };

            myset.Tables.Add(myTB);

            // Création de la table "Joueurs"
            myTB = new DataTable("Joueurs");

            myTB.Columns.Add(new DataColumn("RefJoueur", typeof(long))
            {
                AutoIncrement = true,
                AutoIncrementSeed = 1,
                AutoIncrementStep = 1
            });
            myTB.Columns.Add(new DataColumn("Nom", typeof(string)) { MaxLength = 50 });
            myTB.Columns.Add(new DataColumn("Poste", typeof(string)) { MaxLength = 50 });
            myTB.Columns.Add(new DataColumn("Salaire", typeof(decimal)));
            myTB.Columns.Add(new DataColumn("Description", typeof(string)) { MaxLength = 250 });
            myTB.Columns.Add(new DataColumn("RefEquipe", typeof(long)));
            myTB.PrimaryKey = new[] { myTB.Columns["RefJoueur"] };

            myset.Tables.Add(myTB);

            // Création d'une relation entre "Equipes" et "Joueurs"
            DataRelation myRel = new DataRelation(
                "Equipe_Joueurs",
                myset.Tables["Equipes"].Columns["RefEquipe"],
                myset.Tables["Joueurs"].Columns["RefEquipe"]
            );
            myset.Relations.Add(myRel);

            // Ajout des enregistrements dans la table "Equipes"
            myTB = myset.Tables["Equipes"];

            DataRow myRow = myTB.NewRow();
            myRow["Nom"] = "Real de Madrid";
            myRow["Ville"] = "Madrid (Espagne)";
            myRow["Budget"] = 1600000;
            myRow["Coach"] = "Carlos Flick";
            myTB.Rows.Add(myRow);

            myRow = myTB.NewRow();
            myRow["Nom"] = "Barcelone";
            myRow["Ville"] = "Barcelone (Espagne)";
            myRow["Budget"] = 23698512;
            myRow["Coach"] = "Xavi Hernandez";
            myTB.Rows.Add(myRow);

            myRow = myTB.NewRow();
            myRow["Nom"] = "Bayern";
            myRow["Ville"] = "Munich (Allemagne)";
            myRow["Budget"] = 3695456;
            myRow["Coach"] = "Thomas Tuchel";
            myTB.Rows.Add(myRow);

            myRow = myTB.NewRow();
            myRow["Nom"] = "Celta Vigo";
            myRow["Ville"] = "Valence (Espagne)";
            myRow["Budget"] = 12360000;
            myRow["Coach"] = "Luc";
            myTB.Rows.Add(myRow);

            myRow = myTB.NewRow();
            myRow["Nom"] = "Getafe";
            myRow["Ville"] = "Villarreal (Espagne)";
            myRow["Budget"] = 7580000;
            myRow["Coach"] = "TCB";
            myTB.Rows.Add(myRow);

            // Ajout des joueurs dans la table "Joueurs"
            myTB = myset.Tables["Joueurs"];

            myRow = myTB.NewRow();
            myRow["Nom"] = "KB9";
            myRow["Poste"] = "Avant-centre";
            myRow["Salaire"] = 1500000;
            myRow["Description"] = "Joueur clé du Real Madrid";
            myRow["RefEquipe"] = 1;
            myTB.Rows.Add(myRow);

            myRow = myTB.NewRow();
            myRow["Nom"] = "Pedri";
            myRow["Poste"] = "Milieu";
            myRow["Salaire"] = 750000;
            myRow["Description"] = "Joueur talentueux de Barcelone";
            myRow["RefEquipe"] = 2;
            myTB.Rows.Add(myRow);

            myRow = myTB.NewRow();
            myRow["Nom"] = "Neuer";
            myRow["Poste"] = "Gardien";
            myRow["Salaire"] = 800000;
            myRow["Description"] = "Capitaine du Bayern Munich";
            myRow["RefEquipe"] = 3;
            myTB.Rows.Add(myRow);

            return myset;
        }
    }
}
