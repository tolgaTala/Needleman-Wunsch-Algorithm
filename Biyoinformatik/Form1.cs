using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace Biyoinformatik
{
    public partial class Form1 : Form
    {
        static string[] seq1 = System.IO.File.ReadAllLines(@"C:\Users\tolga\source\repos\Biyoinformatik2\seq1.txt");
        static string[] seq2 = System.IO.File.ReadAllLines(@"C:\Users\tolga\source\repos\Biyoinformatik2\seq2.txt");

        int seq1Count = Convert.ToInt32(seq1[0]) + 1;
        string seq1String = seq1[1];

        int seq2Count = Convert.ToInt32(seq2[0]) + 1;
        string seq2String = seq2[1];

        int match = 1;
        int misMmatch = -1;
        int gap = -2;

        Stopwatch watch = new Stopwatch();

        public Form1()
        {
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text files | *.txt";
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string seq1Path = dialog.FileName;
                seq1 = System.IO.File.ReadAllLines(seq1Path);
                seq1Count = Convert.ToInt32(seq1[0]) + 1;
                seq1String = seq1[1];
                label8.Text = seq1Path;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text files | *.txt";
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string seq2Path = dialog.FileName;
                seq2 = System.IO.File.ReadAllLines(seq2Path);
                seq2Count = Convert.ToInt32(seq2[0]) + 1;
                seq2String = seq2[1];
                label9.Text = seq2Path;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            watch.Reset();
            watch.Start();

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();

            dataGridView1.Columns.Add("asd", " ");
            dataGridView1.Rows.Add();
            dataGridView1.Rows[0].Cells[0].Value = "0".ToString();

            label1.Text = seq1String;
            label2.Text = seq2String;

            for (int i = 0; i < seq1Count - 1; i++)
            {
                dataGridView1.Columns.Add("sembol", seq1String[i].ToString());
            }
            for (int j = 0; j < seq2Count - 1; j++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[j + 1].HeaderCell.Value = seq2String[j].ToString();
            }

            char[] seq2Array = seq2String.ToCharArray();
            char[] seq1Array = seq1String.ToCharArray();

            match = Convert.ToInt32(textBox1.Text);
            misMmatch = Convert.ToInt32(textBox2.Text);
            gap = Convert.ToInt32(textBox3.Text);

            dataGridView1.Rows[0].Cells[0].Value = 0;

            int seq2sayac = 0;
            int seq1sayac = 0;

            int scoreleft2;
            int scoreUp2;
            for (int i = 1; i < seq2Count; i++)
            {
                scoreleft2=Convert.ToInt32(dataGridView1.Rows[i-1].Cells[0].Value) + gap;
                if (scoreleft2 < 0)
                {
                    scoreleft2 = 0;
                }
                dataGridView1.Rows[i].Cells[0].Value = scoreleft2;
            }
            for (int i = 1; i < seq1Count; i++)
            {
                scoreUp2 = Convert.ToInt32(dataGridView1.Rows[0].Cells[i-1].Value) + gap;
                if (scoreUp2<0)
                {
                    scoreUp2 = 0;
                }
                dataGridView1.Rows[0].Cells[i].Value = scoreUp2;
            }

            //Tablodaki uzunlarına göre dönüyor.
            for (int i = 0; i < seq2Count; i++)
            {
                for (int j = 0; j < seq1Count; j++)
                {
                    //sol üst köşede hiç bir işlem yapmadan devam etmesini için
                    if (j == 0 && i == 0)
                    {
                        continue;
                    }

                    int scoreDiag = 0;
                    int scoreleft = 0;
                    int scoreUp = 0;
                    //ilk değer için eşleşme var mı yok mu diye bakıyor.
                    if (seq2Array[seq2sayac] == seq1Array[seq1sayac])
                    {
                        //ikisinden biri 0 olursa demekki scorediag işleme girmemeli 
                        if (i == 0 || j == 0)
                        {
                            scoreDiag = -1000;
                        }
                        else
                        {
                            scoreDiag = Convert.ToInt32(dataGridView1.Rows[i - 1].Cells[j - 1].Value) + match;
                        }
                    }
                    else
                    {
                        if (i == 0 || j == 0)
                        {
                            scoreDiag = -1000;
                        }
                        else
                        {
                            scoreDiag = Convert.ToInt32(dataGridView1.Rows[i - 1].Cells[j - 1].Value) + misMmatch;
                        }

                    }

                    if (i != 0 && j!=1)
                    {
                        scoreleft = Convert.ToInt32(dataGridView1.Rows[i - 1].Cells[j].Value) + gap;
                    }
                    else
                    {
                        if (i == 0 && j == 1)
                        {
                            scoreleft = Convert.ToInt32(dataGridView1.Rows[i].Cells[j - 1].Value) + gap;
                            if (scoreleft < 0)
                            {
                                scoreleft = 0;
                            }
                            dataGridView1.Rows[i].Cells[j].Value = scoreleft;
                        }
                        
                    }
                    if (j != 0 && i!=1)
                    {
                        scoreUp = Convert.ToInt32(dataGridView1.Rows[i].Cells[j - 1].Value) + gap;
                    }
                    else
                    {
                        if (j == 0 && i == 1)
                        {
                            scoreUp = Convert.ToInt32(dataGridView1.Rows[i - 1].Cells[j].Value) + gap;
                            if (scoreUp < 0)
                            {
                                scoreUp = 0;
                            }
                            dataGridView1.Rows[i].Cells[j].Value = scoreUp;
                        }
                        
                    }
                    if (scoreDiag<0)
                    {
                        scoreDiag = 0;
                    }
                    if (scoreleft < 0)
                    {
                        scoreleft = 0;
                    }
                    if (scoreUp < 0)
                    {
                        scoreUp = 0;
                    }
                    int maxScore = Math.Max(Math.Max(scoreDiag, scoreleft), scoreUp);
                    //Üç hesaplama sonucunda maksimum değeri alıp tabloya ekliyoruz.
                    dataGridView1.Rows[i].Cells[j].Value = maxScore;

                    //Sekansı uygun gezmek için eşitliyoruz.
                    seq1sayac = j;
                    //tablo uzunluğumuz sekans uzunluğundan 1 fazla olduğundan out of range yememek için.
                    if (seq1sayac == seq1Array.Length)
                    {
                        seq1sayac--;
                    }
                }
                //aynı şekilde ikinci sekans içinde aynısını uyguluyoruz.
                seq2sayac = i;

                if (seq2sayac == seq2Array.Length)
                {
                    seq2sayac--;
                }
            }


            //Geri takip adımları
            //Hizalama ve maksimum skor bulma

            string HizaliSeq1 = string.Empty;
            string HizaliSeq2 = string.Empty;

            //Sağ alt köşe değerinden başlamak için.
            int m = seq2Count - 1;
            int n = seq1Count - 1;

            var index1 = dataGridView1.Rows[m].Cells[n];
            index1.Style.BackColor = Color.Red;

            int aligmentMaxScore = 0;
            int maxdeger=0;
            for (int i = 0; i <= m; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    if (Convert.ToInt32(dataGridView1.Rows[i].Cells[j].Value)>maxdeger)
                    {
                        maxdeger = Convert.ToInt32(dataGridView1.Rows[i].Cells[j].Value);
                    }
                    
                }
            }

            for (int i = 0; i <= m; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    if (Convert.ToInt32(dataGridView1.Rows[i].Cells[j].Value)==maxdeger)
                    {
                        int x = i;
                        int y = j;
                        int scorediag=0;
                        while (Convert.ToInt32(dataGridView1.Rows[x].Cells[y].Value)!=0)
                        {
                            scorediag = 0;
                                dataGridView1.Rows[x - 1].Cells[y - 1].Style.BackColor = Color.Red;
                                HizaliSeq1 = seq1Array[y - 1] + HizaliSeq1;
                                HizaliSeq2 = seq2Array[x - 1] + HizaliSeq2;
                            scorediag = match;
                            x--;
                            y--;
                            aligmentMaxScore=aligmentMaxScore+scorediag;
                        }
                    }
                    
                }
            }
            label1.Text = HizaliSeq1;
            label2.Text = HizaliSeq2;
            label4.Text = aligmentMaxScore.ToString();

           
            //algoritmanın çalışma süresini hesaplama
            watch.Stop();
            label3.Text = (watch.ElapsedMilliseconds / 1000.0).ToString() + " Milisaniyede tamamlandı.";            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = match.ToString();
            textBox2.Text = misMmatch.ToString();
            textBox3.Text = gap.ToString();           
        }

        
    }
}
