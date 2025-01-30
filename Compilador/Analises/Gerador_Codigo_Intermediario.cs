using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Analises
{
    internal class Gerador_Codigo_Intermediario
    {
        Dictionary<string, Tabela_Operacoes> tabelaOperacoes = new Dictionary<string, Tabela_Operacoes>();
        Dictionary<string, string> tabelaVar = new Dictionary<string, string>();
        List<object> listVar = new List<object>();
        private string codigoIntermedairio = "";
        public Gerador_Codigo_Intermediario(string relatorioLexico)
        {
            string[] textoLexico;
            string tabelaVariaveis = "";
            string tabelaComandos = "";
            int contVar = 0;
            //Trasfomar variavel em varivel do sistema
            if (!relatorioLexico.Contains("ERRO"))
            {
                textoLexico = relatorioLexico.Split('\n');
                for (int i = 0; i < textoLexico.Length; i++)
                {

                    string[] palavras = textoLexico[i].Split(' ');
                    // Console.WriteLine(palavras[(palavras.Length-1)]); 
                    if (!palavras[0].Equals("") && (palavras[2].Equals("t_integer") || palavras[2].Equals("t_float") || palavras[2].Equals("t_char")))
                    {
                        i++;
                        palavras = textoLexico[i].Split(' ');
                        tabelaVar.Add(palavras[0], "VAR" + contVar++);
                    }

                }
            
                Console.WriteLine(tabelaVar);
                //MOntar string de variavel
                /*textoLexico = relatorioLexico.Split('\n');

                for (int i = 0; i < textoLexico.Length; i++)
                {
                    string[] palavras = textoLexico[i].Split(' ');
                    if (!palavras[0].Equals("") && palavras[2].Equals("t_atribuicao"))
                    {
                        string[] aux = textoLexico[--i].Split(' ');
                        int j = ++i;
                        //tabelaVariaveis += "" + palavras[palavras.Length - 1] + " ";
                        while (aux[aux.Length - 1] == palavras[palavras.Length - 1])
                        {

                            tabelaVariaveis += aux[0] + " ";
                            aux = textoLexico[j++].Split(' ');

                        }
                        tabelaVariaveis += "|";
                    }
                }

                Console.WriteLine(tabelaVariaveis);*/
               // Console.WriteLine(tabelaComandos);

                //Montar o codigo intermediario
                /*textoLexico = tabelaVariaveis.Split('|');
                for (int i = 0; i < textoLexico.Length; i++)
                {
                    string[] palavras = textoLexico[i].Split(' ');
                    for (global::System.Int32 j = 0; j < palavras.Length; j++)
                    {
                        if (tabelaVar.TryGetValue(palavras[j], out string val))
                        {
                            textoLexico[i] = textoLexico[i].Replace(palavras[j], val);
                            Console.WriteLine(textoLexico[i]);
                        }
                    }
                   
                }
                contVar = 0;
                for (int i = 0; i < textoLexico.Length; i++)
                {
                    while (textoLexico[i].Contains('*'))
                    {
                        string[] palavras = textoLexico[i].Split(' ');
                        int j = 0;
                        for (; j < palavras.Length && !palavras[j].Equals("*"); j++) ;

                        string var1, var2;
                        if (tabelaVar.TryGetValue(palavras[j-1], out string value))
                        {
                            var1 = value;
                        }
                        else
                        {
                            var1 = palavras[j-1];
                        }

                        if (tabelaVar.TryGetValue(palavras[j + 1], out string valor))
                        {
                            var2 = valor;
                        }
                        else
                        {
                            var2 = palavras[j + 1];
                        }

                        tabelaOperacoes.Add("TMP"+contVar,new Tabela_Operacoes(var1, palavras[j], var2));
                        textoLexico[i]= textoLexico[i].Replace(var1 + " * " + var2, "TMP" + contVar++);
                        //Console.WriteLine(textoLexico[i]);
                    }

                    while (textoLexico[i].Contains('\\'))
                    {
                        string[] palavras = textoLexico[i].Split(' ');
                        int j = 0;
                        for (; j < palavras.Length && !palavras[j].Equals("\\"); j++) ;

                        string var1, var2;
                        if (tabelaVar.TryGetValue(palavras[j - 1], out string value))
                        {
                            var1 = value;
                        }
                        else
                        {
                            var1 = palavras[j - 1];
                        }

                        if (tabelaVar.TryGetValue(palavras[j + 1], out string valor))
                        {
                            var2 = valor;
                        }
                        else
                        {
                            var2 = palavras[j + 1];
                        }

                        tabelaOperacoes.Add("TMP" + contVar, new Tabela_Operacoes(var1, palavras[j], var2));
                        textoLexico[i] = textoLexico[i].Replace(var1 + " \\ " + var2, "TMP" + contVar++);
                       // Console.WriteLine(textoLexico[i]);
                    }

                    while (textoLexico[i].Contains('+'))
                    {
                        string[] palavras = textoLexico[i].Split(' ');
                        int j = 0;
                        for (; j < palavras.Length && !palavras[j].Equals("+"); j++) ;

                        string var1, var2;
                        if (tabelaVar.TryGetValue(palavras[j - 1], out string value))
                        {
                            var1 = value;
                        }
                        else
                        {
                            var1 = palavras[j - 1];
                        }

                        if (tabelaVar.TryGetValue(palavras[j + 1], out string valor))
                        {
                            var2 = valor;
                        }
                        else
                        {
                            var2 = palavras[j + 1];
                        }

                        tabelaOperacoes.Add("TMP" + contVar, new Tabela_Operacoes(var1, palavras[j], var2));
                        textoLexico[i] = textoLexico[i].Replace(var1 + " + " + var2, "TMP" + contVar++);
                        //Console.WriteLine(textoLexico[i]);
                    }

                    while (textoLexico[i].Contains('-'))
                    {
                        string[] palavras = textoLexico[i].Split(' ');
                        int j = 0;
                        for (; j < palavras.Length && !palavras[j].Equals("-"); j++) ;

                        string var1, var2;
                        if (tabelaVar.TryGetValue(palavras[j - 1], out string value))
                        {
                            var1 = value;
                        }
                        else
                        {
                            var1 = palavras[j - 1];
                        }

                        if (tabelaVar.TryGetValue(palavras[j + 1], out string valor))
                        {
                            var2 = valor;
                        }
                        else
                        {
                            var2 = palavras[j + 1];
                        }

                        tabelaOperacoes.Add("TMP" + contVar, new Tabela_Operacoes(var1, palavras[j], var2));
                        textoLexico[i] = textoLexico[i].Replace(var1 + " - " + var2, "TMP" + contVar++);
                       // Console.WriteLine(textoLexico[i]);
                    }
                  
                    foreach (var chave in tabelaOperacoes.Keys)
                    {
                        var valor = tabelaOperacoes[chave];
                        codigoIntermedairio += $"{chave} = {valor.Var1} {valor.Op} {valor.Var2}\n";
                       // Console.WriteLine($"Chave: {chave}, Var1: {valor.Var1}, Op: {valor.Op}, Var2: {valor.Var2}");
                    }
                    codigoIntermedairio += textoLexico[i]+"\n";
                    tabelaOperacoes.Clear();

                }*/

                //WHILE E IF
                Stack<string> pilha = new Stack<string>();
                Queue<string> filaW = new Queue<string>();
                textoLexico = relatorioLexico.Split('\n');
                int contLabel = 0;
                int contLabelW = 0;
                int indice = 0;
                for (int i = 0; i < textoLexico.Length; i++)
                {
                    string[] aux = textoLexico[i].Split(' ');

                    if (!aux[0].Equals("") && aux[2].Equals("t_if")) {
                        textoLexico[i] = textoLexico[i].Replace(aux[0], "jpm");
                    }
                    if (!aux[0].Equals("") && aux[2].Equals("t_while"))
                    {
                        textoLexico[i] = textoLexico[i].Replace(aux[0], "WLB" + contLabel);
                        filaW.Enqueue("WLB" + contLabel);
                        aux = textoLexico[++i].Split(' ');
                        if (!aux[0].Equals("") && aux[0].Equals("("))
                        {
                            textoLexico[i] = textoLexico[i].Replace(aux[0], "jpm");
                        }
                    }
                    if (!aux[0].Equals("") && (aux[0].Equals("==") || aux[2].Equals("!=") || aux[2].Equals("t_menor")
                            || aux[2].Equals("t_maior") || aux[2].Equals("t_maiorigual") || aux[2].Equals("t_menorigual")))
                    {

                        aux = textoLexico[i-1].Split(' ');
                        if (tabelaVar.TryGetValue(aux[0], out string value))
                        {
                            textoLexico[i-1] = textoLexico[i-1].Replace(aux[0], "("+value);
                        }
                        else
                        {
                            textoLexico[i - 1] = ReplaceFirstOccurrence(textoLexico[i - 1], aux[0], "(" + aux[0]);
                        }
                        aux = textoLexico[i + 1].Split(' ');
                        if (tabelaVar.TryGetValue(aux[0], out string valor))
                        {
                            textoLexico[i+1] = textoLexico[i+1].Replace(aux[0], valor+")");
                        }
                        else
                        {
                            textoLexico[i + 1] = ReplaceFirstOccurrence(textoLexico[i + 1], aux[0], aux[0] + ")");
                        }
                    }

                    if (!aux[0].Equals("") && aux[0].Equals(")"))
                    {
                        textoLexico[i] = textoLexico[i].Replace(aux[0], ",");
                    }

                    

                    if (!aux[0].Equals("") && aux[2].Equals("t_inibloco"))
                    {
                        
                        pilha.Push("LB" + contLabel);
                       /* if (pilha.Count > pilhaW.Count)
                            pilhaW.Push("null");*/
                        textoLexico[i] = textoLexico[i].Replace("{", "LB"+contLabel++);
                    }

                    if(!aux[0].Equals("") && aux[2].Equals("t_fimbloco")){
                        //if()
                        textoLexico[i] = textoLexico[i].Replace("}",pilha.Pop());
                    }
                }
               // textoLexico.Slice()
                Console.WriteLine(textoLexico);
                //textoLexico = relatorioLexico.Split('\n');
                contVar = 0;
                for (int i = 0; i < textoLexico.Length; i++)
                {
                    string[] palavras = textoLexico[i].Split(' ');
                    if (!palavras[0].Equals("") && palavras[2].Equals("t_atribuicao"))
                    {
                        string[] aux = textoLexico[--i].Split(' ');
                        int j = ++i;
                        tabelaVariaveis = "";
                        //tabelaVariaveis += "" + palavras[palavras.Length - 1] + " ";
                        while (aux[aux.Length - 1] == palavras[palavras.Length - 1])
                        {

                            tabelaVariaveis += aux[0] + " ";
                            aux = textoLexico[j++].Split(' ');

                        }
                        tabelaVariaveis += "|";

                        string[] textoVaria = tabelaVariaveis.Split('|');
                        for (int k = 0; k < textoVaria.Length; k++)
                        {
                            string[] auxiliar = textoVaria[k].Split(' ');
                            for (global::System.Int32 h = 0; h < auxiliar.Length; h++)
                            {
                                if (tabelaVar.TryGetValue(auxiliar[h], out string val))
                                {
                                    textoVaria[k] = textoVaria[k].Replace(auxiliar[h], val);
                                    Console.WriteLine(textoVaria[k]);
                                }
                            }

                        }
                        
                        for (int k = 0; k < textoVaria.Length; k++)
                        {
                            while (textoVaria[k].Contains('*'))
                            {
                                palavras = textoVaria[k].Split(' ');
                                 j = 0;
                                for (; j < palavras.Length && !palavras[j].Equals("*"); j++) ;

                                string var1, var2;
                                if (tabelaVar.TryGetValue(palavras[j - 1], out string value))
                                {
                                    var1 = value;
                                }
                                else
                                {
                                    var1 = palavras[j - 1];
                                }

                                if (tabelaVar.TryGetValue(palavras[j + 1], out string valor))
                                {
                                    var2 = valor;
                                }
                                else
                                {
                                    var2 = palavras[j + 1];
                                }

                                tabelaOperacoes.Add("TMP" + contVar, new Tabela_Operacoes(var1, palavras[j], var2));
                                textoVaria[k] = textoVaria[k].Replace(var1 + " * " + var2, "TMP" + contVar++);
                                //Console.WriteLine(textoLexico[i]);
                            }

                            while (textoVaria[k].Contains('\\'))
                            {
                                palavras = textoVaria[k].Split(' ');
                                 j = 0;
                                for (; j < palavras.Length && !palavras[j].Equals("\\"); j++) ;

                                string var1, var2;
                                if (tabelaVar.TryGetValue(palavras[j - 1], out string value))
                                {
                                    var1 = value;
                                }
                                else
                                {
                                    var1 = palavras[j - 1];
                                }

                                if (tabelaVar.TryGetValue(palavras[j + 1], out string valor))
                                {
                                    var2 = valor;
                                }
                                else
                                {
                                    var2 = palavras[j + 1];
                                }

                                tabelaOperacoes.Add("TMP" + contVar, new Tabela_Operacoes(var1, palavras[j], var2));
                                textoVaria[k] = textoVaria[k].Replace(var1 + " \\ " + var2, "TMP" + contVar++);
                                // Console.WriteLine(textoLexico[i]);
                            }

                            while (textoVaria[k].Contains('+'))
                            {
                                 palavras = textoVaria[k].Split(' ');
                                 j = 0;
                                for (; j < palavras.Length && !palavras[j].Equals("+"); j++) ;

                                string var1, var2;
                                if (tabelaVar.TryGetValue(palavras[j - 1], out string value))
                                {
                                    var1 = value;
                                }
                                else
                                {
                                    var1 = palavras[j - 1];
                                }

                                if (tabelaVar.TryGetValue(palavras[j + 1], out string valor))
                                {
                                    var2 = valor;
                                }
                                else
                                {
                                    var2 = palavras[j + 1];
                                }

                                tabelaOperacoes.Add("TMP" + contVar, new Tabela_Operacoes(var1, palavras[j], var2));
                                textoVaria[k] = textoVaria[k].Replace(var1 + " + " + var2, "TMP" + contVar++);
                                //Console.WriteLine(textoLexico[i]);
                            }

                            while (textoVaria[k].Contains('-'))
                            {
                                 palavras = textoVaria[k].Split(' ');
                                 j = 0;
                                for (; j < palavras.Length && !palavras[j].Equals("-"); j++) ;

                                string var1, var2;
                                if (tabelaVar.TryGetValue(palavras[j - 1], out string value))
                                {
                                    var1 = value;
                                }
                                else
                                {
                                    var1 = palavras[j - 1];
                                }

                                if (tabelaVar.TryGetValue(palavras[j + 1], out string valor))
                                {
                                    var2 = valor;
                                }
                                else
                                {
                                    var2 = palavras[j + 1];
                                }

                                tabelaOperacoes.Add("TMP" + contVar, new Tabela_Operacoes(var1, palavras[j], var2));
                                textoVaria[k] = textoVaria[k].Replace(var1 + " - " + var2, "TMP" + contVar++);
                                // Console.WriteLine(textoLexico[i]);
                            }

                            foreach (var chave in tabelaOperacoes.Keys)
                            {
                                var valor = tabelaOperacoes[chave];
                                codigoIntermedairio += $"{chave} = {valor.Var1} {valor.Op} {valor.Var2}\n";
                                // Console.WriteLine($"Chave: {chave}, Var1: {valor.Var1}, Op: {valor.Op}, Var2: {valor.Var2}");
                            }
                            codigoIntermedairio += textoVaria[k] + "\n";
                            tabelaOperacoes.Clear();

                        }

                    }

                    
                    if (!palavras[0].Equals("") && palavras[2].Equals("t_if"))
                    {
                        string[] aux = textoLexico[i].Split(' ');
                        int j = i + 1;
                        while (aux[aux.Length - 1] == palavras[palavras.Length - 1])
                        {
                            if(aux[0].Equals(","))
                                codigoIntermedairio += "= FALSE "+aux[0] + " ";
                            else if(!aux[0].Equals("("))
                                codigoIntermedairio += aux[0]+" ";
                            aux = textoLexico[j++].Split(' ');

                        }
                        codigoIntermedairio += "\n";

                    }

                    if (!palavras[0].Equals("") && palavras[2].Equals("t_while"))
                    {
                        string[] aux = textoLexico[i].Split(' ');
                        int j = i + 1;
                        while (aux[aux.Length - 1] == palavras[palavras.Length - 1])
                        {
                            if (aux[2].Equals("t_while"))
                                codigoIntermedairio += aux[0] + ":\n";
                            else if (aux[0].Equals(","))
                                codigoIntermedairio += "= FALSE " + aux[0] + " ";
                            else
                                codigoIntermedairio += aux[0] + " ";
                            aux = textoLexico[j++].Split(' ');

                        }
                        codigoIntermedairio += "\n";

                    }

                    if (!palavras[0].Equals("") && palavras[2].Equals("t_fimbloco"))
                    {
                        string lab = palavras[0];
                       // string strfila = filaW.Dequeue();
                        if (filaW.Count() !=0 && lab[lab.Length - 1].Equals(filaW.Peek()[3]))
                            codigoIntermedairio += "jmp " + filaW.Dequeue() + "\n"; 
                        codigoIntermedairio += palavras[0]+"\n";

                    }





                }
            }


        }
        public string getCodIntermediario()
        {
            return codigoIntermedairio;
        }
        static string ReplaceFirstOccurrence(string source, string substringToReplace, string newSubstring)
        {
            int index = source.IndexOf(substringToReplace);
            if (index < 0)
            {
                // A substring não foi encontrada
                return source;
            }
            // Construir a nova string
            return source.Substring(0, index) + newSubstring + source.Substring(index + substringToReplace.Length);
        }

        //Classe Internar da Tabela_Operacoes
        private class Tabela_Operacoes
        {
            public Tabela_Operacoes(string var1, string op, string var2)
            {

                Var1 = var1;
                Op = op;
                Var2 = var2;
            }
            public string Var1 { get; set; }
            public string Op { get; set; }
            public string Var2 { get; set; }


        }
    }
}
