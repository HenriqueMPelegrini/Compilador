using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Analises
{
    internal class Analise_Semantica
    {
         Dictionary<string, Tabela_Simbolos> tabelaSimbolos = new Dictionary<string, Tabela_Simbolos>();
        

        string erroSemantico = "";

        public Analise_Semantica(string relatorioLexico) {
            string[] textoLexico;
            string tabelaVariaveis ="";
            if (!relatorioLexico.Contains("ERRO"))
            {
                textoLexico = relatorioLexico.Split('\n');
                for (int i = 0; i < textoLexico.Length; i++)
                {

                    string[] palavras = textoLexico[i].Split(' ');
                    // Console.WriteLine(palavras[(palavras.Length-1)]); 
                    if (!palavras[0].Equals("") && (palavras[2].Equals("t_program") || palavras[2].Equals("t_integer") || palavras[2].Equals("t_float") || palavras[2].Equals("t_char")))
                    {
                        Tabela_Simbolos tabela = new Tabela_Simbolos(palavras[2], "var", palavras[0],-1);
                        i++;
                        palavras = textoLexico[i].Split(' ');
                        tabelaSimbolos.Add(palavras[0], tabela);
                    }

                    if (!palavras[0].Equals("") && palavras[2].Equals("t_atribuicao"))
                    {
                        i--;
                        palavras = textoLexico[i].Split(' ');
                        if (tabelaSimbolos.TryGetValue(palavras[0], out Tabela_Simbolos objeto))
                        {
                            // Modifique o valor da propriedade "Valor"
                            objeto.Ultima_linha = int.Parse(palavras[palavras.Length - 1]); ;
                        }
                       /* else
                        {
                            // A chave não foi encontrada no dicionário
                            Console.WriteLine("Chave não encontrada.");
                        }*/
                        i++;
                    }

                }
                textoLexico = relatorioLexico.Split('\n');
              
                for (int i = 0; i < textoLexico.Length; i++)
                {
                    string[] palavras = textoLexico[i].Split(' ');
                    if (!palavras[0].Equals("") && palavras[2].Equals("t_atribuicao"))
                    {
                        string[] aux = textoLexico[--i].Split(' ');
                        int j = ++i;
                        tabelaVariaveis += "" + palavras[palavras.Length - 1]+" ";
                        while (aux[aux.Length-1] == palavras[palavras.Length - 1])
                        {
                            
                            tabelaVariaveis += aux[0]+" ";
                            aux = textoLexico[j++].Split(' ');

                        }
                        tabelaVariaveis += "| ";
                    }
                }

                Console.WriteLine(tabelaVariaveis);

                    //Ver se foi declaarda
                    //Ver se foi utilizada 
                    //Ver se o valor que esta sendo atriuido a ela é correto

                textoLexico = relatorioLexico.Split('\n');
                for (int i = 0; i < textoLexico.Length; i++)
                {
                    string[] palavras = textoLexico[i].Split(' ');
                    if (!palavras[0].Equals("") && palavras[2].Equals("t_id"))
                    {
                        if (tabelaSimbolos.TryGetValue(palavras[0], out Tabela_Simbolos objeto) )
                        {
                            if (objeto.Ultima_linha == -1 && !objeto.Tipo.Equals("Program"))
                            {
                                erroSemantico += "@ERRO: Variável não utilizada => linha : " + palavras[palavras.Length - 1] +"\n";
                            }
                        }
                        else
                        {
                            erroSemantico += "@ERRO: Variável não declarada => linha : "+ palavras[palavras.Length - 1]+"\n";
                        }


                    }
                }

                textoLexico = tabelaVariaveis.Split('|');
                for (int i = 0; i < textoLexico.Length; i++)
                {
                    string[] palavras = textoLexico[i].Split(' ');
                    string tipo;
                    if (tabelaSimbolos.TryGetValue(palavras[1], out Tabela_Simbolos objeto)) { 
                        tipo = objeto.Tipo;
                        int j = 3;
                        for (; j < palavras.Length && tipo.Equals(objeto.Tipo); j++)
                        {
                            if (tabelaSimbolos.TryGetValue(palavras[j], out Tabela_Simbolos ob))
                                tipo = ob.Tipo;
                              
                        }

                        if (j<palavras.Length)
                        {
                            erroSemantico += "@ERRO: Erro de casting '" + objeto.Tipo + "' != '"+tipo+"' => linha : " + palavras[0] + "\n";
                        }
                    }
                   
                }
            }
            Console.WriteLine(tabelaSimbolos);
            Console.WriteLine(erroSemantico);
        }

        public string getErro()
        {
            return erroSemantico;
        }


        //Classe Internar da Tabela_Simbolos
        private class Tabela_Simbolos
        {
            public Tabela_Simbolos(string token, string categoria, string tipo, int ultima_linha)
            {
               
                Token = token;
                Categoria = categoria;
                Tipo = tipo;
                //Valor = valor;
                Ultima_linha = ultima_linha;
            }
            public string Token { get; set; }
            public string Categoria { get; set; }
            public string Tipo { get; set; }
            public string Valor { get; set; }
            public int Ultima_linha { get; set; }

        }
    }
}
