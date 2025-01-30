using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Analises
{
    internal class Gerador_Codigo_Maquina
    {
        //Dictionary<string, string> variableMap = new Dictionary<string, string>();
        public Gerador_Codigo_Maquina(string codIntermediario) {

            string codMaquina = "";
            // Código intermediário fornecido
            string[] intermediateCode = {
             "VAR0 = 2",
             "VAR1 = 3",
             "TMP0 = VAR0 + VAR1",
             "VAR3 = TMP0",
             "TMP1 = VAR0 - 1",
             "VAR3 = TMP1",
             "VAR0 = TMP0",
             "jpm (2 == VAR0) = FALSE , LB0",
             "WLB1:",
             "jpm (VAR0 == 2) = FALSE , LB1",
             "jmp WLB1",
             "LB1",
             "LB0"
             };
            //string[] intermediateCode = codIntermediario.Split('\n');
            List<string> simpsimCode = ConvertToMachine(intermediateCode);
           
            foreach (string line in simpsimCode)
            {
                if (line.Contains("\n"))
                {
                    string[] palavras = line.Split('\n');
                    for (global::System.Int32 i = 0; i < palavras.Length; i++)
                    {
                        codMaquina += palavras[i]+"\n";
                    }
                }
                else
                {
                    codMaquina += line+"\n";
                }
            }
            Console.WriteLine(codMaquina);
        }

        

        public List<string> ConvertToMachine(string[] intermediateCode)
        {
            var simpsimCode = new List<string>();
            Dictionary<string,string> variableMap = new Dictionary<string, string>();
          /*  {
                { "VAR0", "R0" },
                { "VAR1", "R1" },
                { "TMP0", "R2" },
                { "TMP1", "R3" },
                { "VAR3", "R4" }
            };*/
            int r = 1;
            foreach (var line in intermediateCode)
            {
                if (!line.Equals("") && (line.ElementAt(0).Equals('T') || line.ElementAt(0).Equals('V')))
                {
                    string[] palavras = line.Split(' ');
                    if (!variableMap.ContainsKey(palavras[0]))
                    {
                        variableMap.Add(palavras[0], "R" + (r++));
                    }
                   

                }
            }
           /* if (variableMap.TryGetValue("VAR3", out string valor))
                Console.WriteLine("aaaaaaaaaaaaaaaaa"+ valor);*/

            foreach (var line in intermediateCode)
            {
                string translatedLine = TranslateLine(line,variableMap,r);
                if (!string.IsNullOrEmpty(translatedLine))
                {
                    simpsimCode.Add(translatedLine);
                }
            }

            return simpsimCode;
        }

        public string TranslateLine(string line,Dictionary<string,string> variableMap, int r)
        {
            // Dictionary<string, string> variableMap = new Dictionary<string, string>(var);
           
            //string[] parts = line.Split(new char[] { ' ', '=', '+', '-', '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] parts = line.Split(new char[] { ' ','(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 3 && parts[1] == "=") // Assignment operation
            {
                if (int.TryParse(parts[2], out _)) // Immediate load
                {
                    return $"load {variableMap[parts[0]]}, {parts[2]}";
                }
                else if (variableMap.ContainsKey(parts[2])) // Move
                {
                    variableMap.TryGetValue(parts[0], out string val);
                    return $"move {val}, {variableMap[parts[2]]}";
                }
            }
            else if (parts.Length == 5 && parts[3] == "+") // Addition
            {
                return $"addi {variableMap[parts[0]]}, {variableMap[parts[2]]}, {variableMap[parts[4]]}";
            }
            else if (parts.Length == 5 && parts[3] == "-") // Subtraction
            {
                string tempReg = "RF"; // Temporary register to hold -1
                return $"load {tempReg}, -1d\naddi {variableMap[parts[0]]}, {variableMap[parts[2]]}, {tempReg}";
            }
            else if (line.StartsWith("jpm")) // Conditional jump
            {
                if (line.Contains("==")) // Jump when equal
                {
                    string[] jumpParts = line.Split(new char[] { '(', ')', '=', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string condition;
                    string targetLabel = jumpParts[5].Trim();
                    string retorno = "";

                    condition = jumpParts[2].Trim();
                    if (variableMap.ContainsKey(condition.Split(' ')[0]))
                    {
                        retorno += $"load R0,{variableMap[condition.Split(' ')[0]]}\n";
                    }
                    else
                    {
                        retorno += $"load R0,{condition.Split(' ')[0]}\n";
                    }
                    condition = jumpParts[1].Trim();
                    if (variableMap.ContainsKey(condition.Split(' ')[0]))
                    {
                       // retorno += $"jmpEQ {variableMap[condition.Split(' ')[0]]}=";
                        retorno += $"jmpEQ {variableMap[condition.Split(' ')[0]]}=R0, {targetLabel}";
                    }
                    else
                    {
                        retorno += $"load R{r}, {condition.Split(' ')[0]}\n";
                        //retorno += $"jmpEQ R{r++}";
                        retorno += $"jmpEQ R{r++}=R0, {targetLabel}";
                    }
                   
                    return retorno;
                }
            }
            else if (line.StartsWith("jmp")) // Unconditional jump
            {
                return $"jmp {line.Split(' ')[1]}";
            }
            else if (line.EndsWith(":")) // Label
            {
                return line;
            }else
            {
                return line + ":";
            }

            return null;
        }
    }
}
