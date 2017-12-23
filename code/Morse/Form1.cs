using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Morse
{


    public partial class Form1 : Form
    {
        Stopwatch timer = new Stopwatch();
        Stopwatch timer2 = new Stopwatch();
        string textoDec;
        public Form1()
        {
            InitializeComponent();
        }

        public Arbol crearArbol(List<Arbol> codificacion)
        {
            //aqui construye el arbol
            Arbol decodif = new Arbol();

            for (int i = 0; i < codificacion.Count; i++)
            {

                string letra = codificacion[i].letra;

                string codigoBin = codificacion[i].clave;

                Arbol temp = decodif;

                for (int j = 0; j < codigoBin.Length; j++)
                {
                    if (codigoBin[j] == '0')
                    {
                        if (temp.hijoIzq == null)
                            temp.hijoIzq = new Arbol();
                        temp = temp.hijoIzq;
                    }
                    else
                    {
                        if (temp.hijoDer == null)
                            temp.hijoDer = new Arbol();
                        temp = temp.hijoDer;
                    }

                    if (j == codigoBin.Length - 1)
                        temp.letra = letra;

                }
                temp = decodif;
            }
            return decodif;
        }

        public static string decodificacion(Arbol codificacion, string texto)
        {
            string textDec = "";
            Arbol reco = codificacion;
            int k = 0;
            while (k < texto.Length)
            {
                if (texto[k] == '0')
                    reco = reco.hijoIzq;
                else
                    reco = reco.hijoDer;
                if (k == texto.Length - 1)
                {
                    textDec = reco.letra;
                }
                k++;
            }

            return textDec;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
                timer.Start();
                timer2.Stop();
                if (timer2.ElapsedMilliseconds > 1000 && timer2.ElapsedMilliseconds < 3000)
                {
                    textBox1.Text += (" ");
                    textBox2.Text += (" ");
                }
                else if (timer2.ElapsedMilliseconds > 3000)
                {
                    textBox1.Text += ("    ");
                    textBox2.Text += ("    ");
                }

                    timer2.Reset();
            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox1.Text = traducir();
                textBox2.Text = textBox1.Text;
            }
            else
            {
                timer2.Start();
                timer.Stop();

                if (timer.ElapsedMilliseconds < 300)
                {
                    textBox1.Text += ("0");
                    textBox2.Text += (".");
                }
                else //if (timer.ElapsedMilliseconds > 300 && timer.ElapsedMilliseconds < 1000)
                {
                    textBox1.Text += ("1");
                    textBox2.Text += ("-");
                }
                /* else if (timer.ElapsedMilliseconds < 2000)
                 {
                     textoDec += traducir();
                     textBox1.Clear();
                 }
                 else
                     textBox1.Text = textoDec;*/
                //textBox1.Text = Convert.ToString(timer.ElapsedMilliseconds);
                timer.Reset();
            }
        }

        private string traducir()
        {
            string[] textCod;
            string texto;
            List<Arbol> codific = new List<Arbol>();
            try
            {             
                StreamReader sr = new StreamReader("morseDictionary.txt");
                texto = sr.ReadLine();
                while (texto != null)
                {                 
                    Arbol temp = new Arbol();
                    temp.letra = texto;
                    texto = sr.ReadLine();
                    temp.clave = texto;
                    codific.Add(temp);
                    Console.WriteLine(temp.letra + " " + temp.clave);
                    texto = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("excepcion : " + ex.Message);
            }

            Arbol arbol = crearArbol(codific);
            textCod = textBox1.Text.Split(' ');

            for (int i = 0; i < textCod.Length; i++)
                textoDec += decodificacion(arbol, textCod[i]);
                
            return textoDec;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = traducir();
        }
    }

    public class Arbol
    {
        public string clave;
        public string letra;
        public Arbol hijoIzq;
        public Arbol hijoDer;

        public Arbol()
        {
            hijoIzq = null;
            hijoDer = null;
        }


        public virtual void colocaIzq(Arbol V, Arbol t)
        {
            t.hijoIzq = V;
        }

        public virtual void colocaDer(Arbol V, Arbol t)
        {
            t.hijoDer = V;
        }

        public virtual void preOrden(Arbol A)
        {

            if (A != null)
            {
                Console.WriteLine(A.letra + "	" + A.clave + " ");
                preOrden(A.hijoIzq);
                preOrden(A.hijoDer);
            }
        }

    }
}
