using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;

namespace XML_Handler
{
    public class Handler
    {
        string inputPath = "";
        string outputPath = "";

        public Handler(string inputPath, string outputPath)
        {
            this.inputPath = inputPath;
            this.outputPath = outputPath;
        }

        delegate string AdditionalStringOperations(string stringForHandling);

        public void DoEverything()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            Repo repo = new Repo();

            XDocument output = new();
            XElement arrayOfXDictionary = new("ArrayOfXDictionary");
            XElement xDictionary = new("XDictionary");

            XElement fileName = new("FileName", "Equitable- Favorite Healthcare Staffing Inc - Proposal 3.pdf");
            XElement carrier = new("Carrier", "Equitable");
            XElement namedInsured = new("NamedInsured", "Case Proposals");
            XElement lineOfCoverage = new("LineOfCoverage", "Critical Illness");

            xDictionary.Add(fileName);
            xDictionary.Add(carrier);
            xDictionary.Add(namedInsured);
            xDictionary.Add(lineOfCoverage);

            XPathDocument document = new XPathDocument(inputPath);
            XPathNavigator navigator = document.CreateNavigator();
            XmlNamespaceManager manager = new XmlNamespaceManager(navigator.NameTable);

            manager.AddNamespace("cp", "http://classifyprocess.com/2018/07/");
            manager.AddNamespace("cpv", "http://classifyprocess.com/2018/12/visual");
            manager.AddNamespace("docset", "http://www.docugami.com/2021/dgml/Nelligan/CaseProposals10.16.23");
            manager.AddNamespace("addedChunks", "http://www.docugami.com/2021/dgml/Nelligan/CaseProposals10.16.23/addedChunks");
            manager.AddNamespace("dg", "http://www.docugami.com/2021/dgml");
            manager.AddNamespace("dgc", "http://www.docugami.com/2021/dgml/docugami/contracts");
            manager.AddNamespace("dgm", "http://www.docugami.com/2021/dgml/docugami/medical");
            manager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            manager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");

            foreach (var item in repo.parameters)
            {
                switch (item.tagName)
                {
                    case "AgeReduction":
                        GetValueWithXpath(item.tagName, item.xPath, AgeReductionHandler);
                        break;
                    case "Portability":
                        GetValueWithXpath(item.tagName, item.xPath, PortabilityHandler);
                        break;
                    case "EmployerContributionLevel":
                        GetValueWithXpath(item.tagName, item.xPath, EmployerContributionLevelHandler);
                        break;
                    case "AgeBasis":
                        GetValueWithXpath(item.tagName, item.xPath, AgeBasisHandler);
                        break;
                    case "ParticipationRequirement":
                        GetValueWithXpath(item.tagName, item.xPath, ParticipationRequirementHandler);
                        break;
                    case "IsWellnessBenefitCostIncluded":
                        GetValueWithXpath(item.tagName, item.xPath, IsWellnessBenefitCostIncludedHandler);
                        break;
                    case "RateGuarantee":
                        GetValueWithXpath(item.tagName, item.xPath, RateGuaranteeHandler);
                        break;
                    default:
                        GetValueWithXpath(item.tagName, item.xPath, textValue => textValue);
                        break;
                }
            }

            foreach (var item in dictionary)
            {
                XElement element = new(item.Key, item.Value);
                xDictionary.Add(element);
            }

            arrayOfXDictionary.Add(xDictionary);
            output.Add(arrayOfXDictionary);
            output.Save(outputPath);

            // Inner functions
            void GetValueWithXpath(string name, string xpath, AdditionalStringOperations additionalStringOperations)
            {
                XPathExpression query = navigator.Compile(xpath);
                query.SetContext(manager);
                XPathNodeIterator nodes = navigator.Select(query);

                nodes.MoveNext();

                XPathNavigator? nodesNavigator = nodes.Current;
                XPathNodeIterator? nodesText = nodesNavigator?.SelectDescendants(XPathNodeType.Text, false);

                string temp = "";

                if (nodesText?.Current?.Value != null)
                {
                    while (nodesText.MoveNext())
                    {
                        temp += nodesText.Current.Value;
                        temp += " ";
                    }

                    string trimmedTemp = temp.TrimEnd();
                    string completedValue = additionalStringOperations.Invoke(trimmedTemp);

                    dictionary.Add(name, completedValue);
                }
                else
                {
                    Console.WriteLine($"Tagname {name} gives NULL");
                }
            }

            string AgeReductionHandler(string textForHandle)
            {
                Regex regex = new Regex(@"No\sage\sreductions");

                if (regex.IsMatch(textForHandle))
                {
                    return "None";
                }
                else
                {
                    return "Are present age reductions";
                }
            }

            string PortabilityHandler(string textForHandle)
            {
                Regex regex = new Regex(@"70");

                if (regex.IsMatch(textForHandle))
                {
                    return "Included";
                }
                else
                {
                    return "Partial included";
                }
            }

            string EmployerContributionLevelHandler(string textForHandle)
            {
                Regex regex = new Regex(@"0%");

                if (regex.IsMatch(textForHandle))
                {
                    return "Voluntary";
                }
                else
                {
                    return textForHandle;
                }
            }

            string AgeBasisHandler(string textForHandle)
            {
                Regex regex = new Regex(@"^Attained");

                if (regex.IsMatch(textForHandle))
                {
                    return "Attained";
                }
                else
                {
                    return "No attained";
                }
            }

            string ParticipationRequirementHandler(string textForHandle)
            {
                Regex quantityEmployeesRegex = new Regex(@"\s\d{1,2}\s");
                Regex percentageEmployeesRegex = new Regex(@"\s\d{1,3}%");

                Match quantityMatch = quantityEmployeesRegex.Match(textForHandle);
                Match percentageMatch = percentageEmployeesRegex.Match(textForHandle);

                if (quantityMatch.Success && percentageMatch.Success)
                {
                    return $">{percentageMatch.Value.Trim()} or {quantityMatch.Value.Trim()} Lives";
                }
                else
                {
                    return "No participation requirements conditions";
                }
            }

            string IsWellnessBenefitCostIncludedHandler(string textForHandle)
            {
                Regex regex = new Regex(@"\$\d{1,2}\.\d{1,2}");

                if (regex.IsMatch(textForHandle))
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }

            string RateGuaranteeHandler(string textForHandle)
            {
                Regex regex = new Regex(@"\d{1,2}\s");
                Match match = regex.Match(textForHandle);

                if (match.Success)
                {
                    string valueString = match.Value.Trim();
                    int value = int.Parse(valueString);
                    int years = value / 12;
                    int months = value % 12;

                    if (years == 0)
                    {
                        return $"{months} Months";
                    }
                    else if (months == 0)
                    {
                        return $"{years} Years";
                    }
                    else
                    {
                        return $"{years} Years and {months} Months";
                    }

                }
                else
                {
                    return "No specified period";
                }
            }
        }
    }
}
