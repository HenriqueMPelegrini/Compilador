using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Analises
{
    internal class Otimizador_Codigo
    {
        Dictionary<string, Tabela_VarTemporarias> tabelaOperacoes = new Dictionary<string, Tabela_VarTemporarias>();
        Dictionary<string, Tabela_Variaveis> tabelaVariaveis = new Dictionary<string, Tabela_Variaveis>();
        string codOtimizado = "";
        public Otimizador_Codigo(string relatorioInter)
        {
            string[] textointer= relatorioInter.Split('\n');


            for (int i = 0; i < textointer.Length; i++)
            {
                string[] expre = textointer[i].Split(' ');
                //Console.WriteLine(expre[0].ElementAt(0).Equals('T'));
                if (!expre[0].Equals("") && expre[0].ElementAt(0).Equals('T') )
                {
                    if(tabelaOperacoes.Count() == 0)
                        tabelaOperacoes.Add(expre[0], new Tabela_VarTemporarias(expre[2], expre[3], expre[4], i + 1));
                    else
                    {
                        //tabelaOperacoes.TryGetValue(expre[0], out Tabela_VarTemporarias valor);
                        if (expre.Contains("0"))
                        {
                            if(expre.Contains("+") )
                            {
                                textointer[i] = textointer[i].Replace("0", "");
                                textointer[i] = textointer[i].Replace("+", "");
                            }

                            if (expre.Contains("-"))
                            {
                                textointer[i] = textointer[i].Replace("0", "");
                                textointer[i] = textointer[i].Replace("-", "");
                            }
                        }
                        if (expre.Contains("1"))
                        {
                            if (expre.Contains("*"))
                            {
                                textointer[i] = textointer[i].Replace("1", "");
                                textointer[i] = textointer[i].Replace("*", "");
                            }

                            if (expre.Contains("\\"))
                            {
                                textointer[i] = textointer[i].Replace("1", "");
                                textointer[i] = textointer[i].Replace("\\", "");
                            }
                        }

                        Boolean flag = false;
                        var chaves = tabelaOperacoes.Keys.ToList(); // Cria uma cópia das chaves
                        foreach (var chave in chaves)
                        {
                            var valor = tabelaOperacoes[chave];
                            
                            if (!expre[0].Equals("") && valor.Var1.Equals(expre[2]) && valor.Op.Equals(expre[3]) && valor.Var2.Equals(expre[4]))
                            {
                                int linha = 0;
                                if(tabelaVariaveis.TryGetValue(expre[2], out Tabela_Variaveis var1))
                                {
                                    linha = var1.Linha;
                                }

                                if (tabelaVariaveis.TryGetValue(expre[4], out Tabela_Variaveis var2))
                                {
                                    if (var2.Linha > linha)
                                    {
                                        linha = var2.Linha;
                                    }
                                }
                                if(valor.Linha > linha)
                                {
                                    for (global::System.Int32 j = i + 1; j < textointer.Length; j++)
                                    {
                                        if (textointer[j].Contains(expre[0]))
                                        {
                                            textointer[j] = textointer[j].Replace(expre[0], chave);
                                        }
                                    }
                                    textointer[i] = "";
                                    flag = true;
                                }
                                
                            }

                            
                        }
                        if(!flag)
                            tabelaOperacoes.Add(expre[0], new Tabela_VarTemporarias(expre[2], expre[3], expre[4], i + 1));
                    }
                }
                if (!expre[0].Equals("") && expre[0].ElementAt(0).Equals('V') )
                {
                    
                    if(tabelaVariaveis.Count() == 0)
                        tabelaVariaveis.Add(expre[0], new Tabela_Variaveis(expre[2], i + 1));
                    else
                    {
                        if (tabelaVariaveis.TryGetValue(expre[0], out Tabela_Variaveis value))
                        {
                            
                            if (value.Var1.Equals(expre[2]))
                            {
                                if (tabelaVariaveis.TryGetValue(expre[2], out Tabela_Variaveis valor) && value.Linha > valor.Linha)
                                {
                                    textointer[i] = "";
                                }
                                else
                                {
                                    value.Linha = i + 1;
                                    value.Var1 = expre[2];
                                }
                                
                            }
                            else
                            {
                                value.Linha = i + 1;
                                value.Var1 = expre[2];
                            }


                        }
                        else
                        {
                            if(tabelaVariaveis.TryGetValue(expre[2], out Tabela_Variaveis val))
                            {
                                if(tabelaVariaveis.TryGetValue(val.Var1, out Tabela_Variaveis val1) )
                                {
                                    if( val1.Linha > val.Linha)
                                        tabelaVariaveis.Add(expre[0], new Tabela_Variaveis(expre[2], i + 1));
                                    else
                                    {
                                        //string[] expre = textointer[i].Split(' ');
                                        int j = val.Linha - 1;
                                        for (; j < i && !textointer[j].Contains(expre[2]); j++) ;

                                        if (j < i)
                                        {
                                            textointer[val.Linha - 1] = "";
                                            tabelaVariaveis.Add(expre[0], new Tabela_Variaveis(val.Var1, i + 1));
                                            textointer[i]=textointer[i].Replace(expre[2], val.Var1);
                                        }
                                        else
                                        {
                                            tabelaVariaveis.Add(expre[0], new Tabela_Variaveis(expre[2], i + 1));
                                        }
                                    }
                                }
                                else
                                {
                                    tabelaVariaveis.Add(expre[0], new Tabela_Variaveis(expre[2], i + 1));
                                }
                            }
                            else
                            {
                                tabelaVariaveis.Add(expre[0], new Tabela_Variaveis(expre[2], i + 1));
                            }
                            
                        }
                    }

                }
            }
            for (int i = 0; i < textointer.Length; i++)
            {
                if (!textointer[i].Equals(""))
                {
                    codOtimizado += textointer[i] + "\n";
                }
            }

        }

        public string getOtimizador()
        {
            return codOtimizado;
        }


        //Classe Internar da Tabela_Operacoes
        private class Tabela_VarTemporarias
        {
            public Tabela_VarTemporarias(string var1, string op, string var2, int linha)
            {

                Var1 = var1;
                Op = op;
                Var2 = var2;
                Linha = linha;
            }
            public string Var1 { get; set; }
            public string Op { get; set; }
            public string Var2 { get; set; }
            public int Linha { get; set; }


        }

        private class Tabela_Variaveis
        {
            public Tabela_Variaveis(string var1, int linha)
            {

                Var1 = var1;
                Linha = linha;
            }
            public string Var1 { get; set; }
            public int Linha { get; set; }


        }
    }
}

