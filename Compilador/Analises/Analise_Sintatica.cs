using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Analises
{
   
    internal class Analise_Sintatica
    {
        string[,] tbComando;
        int cont,contLinha;
        string[] textoLexico;
        string erroSintatico;

        public Analise_Sintatica(string relatorioLexico)
        {
           // criarTabelaComando();
            cont =0;
            contLinha = 1;
            textoLexico = relatorioLexico.Split('\n');

        }
        public string getErroSintatico()
        {
            return erroSintatico;
        }
        private void criarTabelaComando()
        {
            tbComando = new string[,] {
                {"c_program","t_program t_id t_defvar t_comando"},
                {"t_defvar ","((t_integer | t_float | t_char | t_string) t_id [‘,’ t_id]*)*"},
                { "t_comando", "c_while | c_if | c_else  | c_atribuicao " },
                { "t_bloco", "t_inibloco t_comando(t_comando)* t_fimbloco" },
                { "c_while ", "t_while (t_abrepare (t_oprelacao | t_oplogi) t_fechaparen) (t_bloco)" },
                { "c_if ", "t_if t_abreparen (t_oprelacao | t_oplogi) t_fechaparen (t_bloco)" },
                { "c_else", "t_else t_bloco" },
                { "t_oprelacao ", "t_igual | t_menor [t_menorigual] | t_maior [t_maiorigual]| t_dif" },
                { "t_oplogi", "t_or | t_and | t_not" },
            };
        }

        public void AnalisadorSintatico()
        {         
             c_program();     
        }

        private void c_program()
        {
            string[] palavras = textoLexico[cont++].Split(' ');
            if (!palavras[2].Equals("t_program"))
            {
                erroSintatico += "@ERRO : Falta a palavra 'Program' => linha :"+contLinha + "\n";
                cont--;
            }
            palavras = textoLexico[cont++].Split(' ');
            if (!palavras[2].Equals("t_id"))
            {
                erroSintatico += "@ERRO : Falta a palavra 'ID' => linha :" + contLinha + "\n";
                cont--;
            }
            t_defvar();
            cont--;
            t_comando();
        }

        private void t_defvar()
        {
            int i;
            ;
            string[] palavras = textoLexico[cont++].Split(' ');
            while (palavras[0] != "" && (palavras[2].Equals("t_integer") || palavras[2].Equals("t_float")
                    || palavras[2].Equals("t_char") || palavras[2].Equals("t_string")))
            {
                contLinha++;
                palavras = textoLexico[cont++].Split(' ');
                while ( palavras[0] != "" && (palavras[2].Equals("t_id") || palavras[2].Equals("t_virgula")))
                {         
                    palavras = textoLexico[cont++].Split(' ');
                }
                cont--;
                palavras = textoLexico[--cont].Split(' ');
                if (palavras[0] != "" && !(palavras[2].Equals("t_id") )) //|| palavras[2].Equals("t_virgula")
                {
                    erroSintatico += "@ERRO : Falta a palavra 'ID' ou ',' => linha :" + contLinha + "\n";
                }
                cont++;

                palavras = textoLexico[cont++].Split(' ');
            }

        }

        private void t_comando()
        {
            string[] palavras = textoLexico[cont++].Split(' ');

            if (!palavras[0].Equals("") && palavras[2].Equals("t_while"))
            {
                c_while();
            } else if (!palavras[0].Equals("") && palavras[2].Equals("t_if"))
            {
                c_if();
            }
            else if (!palavras[0].Equals("") && palavras[2].Equals("t_else"))
            {
                c_else();
            }
            else if (!palavras[0].Equals("") && palavras[2].Equals("t_bloco"))
            {
                c_bloco();
            }else if (!palavras[0].Equals("") && palavras[2].Equals("t_atribuicao"))
            {
                // Chamar c_atribuição e fazer cont--´para verficar se tem um id anteriormente
            }else
            {
                if (!palavras[0].Equals("") && !palavras[2].Equals("t_fimbloco"))
                   erroSintatico += "@ERRO : Falta a palavra 'While' ou 'If' => linha :" + contLinha + "\n";
                cont--;
            }
        }

        private void c_while()
        {
            contLinha++;
            string[] palavras = textoLexico[cont++].Split(' ');
            if (!palavras[2].Equals("t_abreparen"))
            {
                erroSintatico += "@ERRO : Falta a palavra '(' => linha :" + contLinha + "\n";
                cont--;
            }
            palavras = textoLexico[cont++].Split(' ');
            if (!palavras[2].Equals("t_id") && !palavras[2].Equals("t_num"))
            {
                erroSintatico += "@ERRO : Falta a palavra 'ID' ou 'numero' => linha :" + contLinha + "\n";
                cont--;
            }
            palavras = textoLexico[cont++].Split(' ');
            
            if (!palavras[2].Equals("t_igual") && !palavras[2].Equals("t_menor") && !palavras[2].Equals("t_menorigual")
                 && !palavras[2].Equals("t_maior") && !palavras[2].Equals("t_maiorigual") && !palavras[2].Equals("t_dif")
                 && !palavras[2].Equals("t_or") && !palavras[2].Equals("t_and") && !palavras[2].Equals("t_not"))
            {
                erroSintatico += "@ERRO : Falta a palavra de operação logica ou de relação => linha :" + contLinha + "\n";
                cont--;
            }
            palavras = textoLexico[cont++].Split(' ');
            if (!palavras[2].Equals("t_id") && !palavras[2].Equals("t_num"))
            {
                erroSintatico += "@ERRO : Falta a palavra 'ID' ou 'numero' => linha :" + contLinha + "\n";
                cont--;
            }

            palavras = textoLexico[cont++].Split(' ');
            if (!palavras[2].Equals("t_fechaparen"))
            {
                erroSintatico += "@ERRO : Falta a palavra ')' => linha :" + contLinha + "\n";
                cont--;
            }
            c_bloco();
        }
        private void c_if()
        {
            contLinha++;
            string[] palavras = textoLexico[cont++].Split(' ');
            if (!palavras[2].Equals("t_abreparen"))
            {
                erroSintatico += "@ERRO : Falta a palavra '(' => linha :" + contLinha + "\n";
                cont--;
            }
            palavras = textoLexico[cont++].Split(' ');
            if (!palavras[2].Equals("t_id") && !palavras[2].Equals("t_num"))
            {
                erroSintatico += "@ERRO : Falta a palavra 'ID' ou 'numero' => linha :" + contLinha + "\n";
                cont--;
            }
            palavras = textoLexico[cont++].Split(' ');
         
            if (!palavras[2].Equals("t_igual") && !palavras[2].Equals("t_menor") && !palavras[2].Equals("t_menorigual")
                 && !palavras[2].Equals("t_maior") && !palavras[2].Equals("t_maiorigual") && !palavras[2].Equals("t_dif")
                 && !palavras[2].Equals("t_or") && !palavras[2].Equals("t_and") && !palavras[2].Equals("t_not"))
            {
                erroSintatico += "@ERRO : Falta a palavra de operação logica ou de relação => linha :" + contLinha + "\n";
                cont--;
            }
            palavras = textoLexico[cont++].Split(' ');
            if (!palavras[2].Equals("t_id") && !palavras[2].Equals("t_num"))
            {
                erroSintatico += "@ERRO : Falta a palavra 'ID' ou 'numero' => linha :" + contLinha + "\n";
                cont--;
            }

            palavras = textoLexico[cont++].Split(' ');
            if (!palavras[2].Equals("t_fechaparen"))
            {
                erroSintatico += "@ERRO : Falta a palavra ')' => linha :" + contLinha + "\n";
                cont--;
            }
            c_bloco();
        }
        private void c_else()
        {
            c_bloco();
        }
        private void c_bloco()
        {
            contLinha++;
            string[] palavras = textoLexico[cont++].Split(' ');
            if (!palavras[2].Equals("t_inibloco"))
            {
                erroSintatico += "@ERRO : Falta a palavra '{' => linha :" + contLinha + "\n";
                cont--;
            }
            contLinha++;
            /*palavras = textoLexico[cont++].Split(' ');*/
            t_comando();
            palavras = textoLexico[cont++].Split(' ');
            contLinha++;
            if (!palavras[2].Equals("t_fimbloco"))
            {
                erroSintatico += "@ERRO : Falta a palavra '}' => linha :" + contLinha + "\n";
                cont--;
            }
           

        }
       
    }
}
