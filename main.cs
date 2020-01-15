using System;
using System.Collections.Generic;
using System.Linq;
					
public class Program
{
	public static void Main()
	{
		string a = "B2,E5,F6";
		string b = "A1,B2,C3,D4";
		string c = "D4,G7,I9";
		string d = "G7,H8";

		List<string> data = new List<string>();
		data.Add(a);
		data.Add(b);
		data.Add(c);
		data.Add(d);
		
		string output = Draw(data, "", "");
		Console.WriteLine(output);
	}
	
	public static string Draw (List<string> orgs, string org, string prefix = "") {
	  if (string.IsNullOrEmpty(org)) 
    {
      int firstOrgIndex = GetLargestOrgIndex(orgs);
      org = orgs[firstOrgIndex];
	  } 
	  List<string> subOrgs = GetSubOrgs(orgs, org);
	  if (subOrgs.Count == 0) 
    {
	    string result = "";
		  List<string> workers = GetWorkers(org);
		  
      for (int i = 0; i < workers.Count; i++)
      {
        if (i == 0) 
        {
          result = result + GetManager(org) + "  " + workers[i] + "\n"; 
          continue;
        }
        if (i == workers.Count - 1) 
        {
          result = result + prefix + "    " + workers[i] + "\n"; 
          continue;
        }

        result = result + prefix + "    " + workers[i] + "\n"; 
      }
		  return result;
	  } 
    else 
    {
		string newPrefix = "    " + prefix;
		string result = "";
		List<string> subManagers = subOrgs.Select(GetManager).ToList();
		List<string> notManagerWorkers = GetWorkers(org).Where(s => subManagers.IndexOf(s) == -1).ToList();
		 
		for (int i = 0; i < subOrgs.Count; i++)
		{
			if (i == 0) 
      {
				result = result + GetManager(org) + "  " + Draw(orgs, subOrgs[i], newPrefix); 
				continue;
			}
			if (i == subOrgs.Count - 1 && notManagerWorkers.Count == 0) 
      {
				result = result + "    " + Draw(orgs, subOrgs[i], newPrefix); 
				continue;
			}

			result = result + "    " + Draw(orgs, subOrgs[i], newPrefix); 
		}
		
		for (int i = 0; i < notManagerWorkers.Count; i++)
		{
			if (i == notManagerWorkers.Count - 1) 
      {
				result = result + prefix + "    " + notManagerWorkers[i] + "\n"; 
			} 
      else 
      {
				result = result + prefix + "    " + notManagerWorkers[i] + "\n"; 
			}
		}
		return result;
	  }
	}
	
	public static int GetLargestOrgIndex (List<string> orgs) {
		List<int> weights = new List<int>();
		for (int i = 0; i < orgs.Count; i++)
		{
			weights.Add(GetOrgSize(orgs, orgs[i]));
		}
		int largest = weights.Max();
		int indexOfLargest = weights.IndexOf(largest);
		return indexOfLargest;
	}
	
	public static string GetManager (string org) {
		List<string> members = GetMembers(org);
		return members[0];
	}

	public static List<string> GetMembers (string org) {
		return org.Split(',').ToList();
	}

	public static List<string> GetWorkers (string org) {
		List<string> members = GetMembers(org);
		List<string> workers = members.Skip(1).Take(members.Count).ToList();
		return workers; 
	}
	
	public static List<string> GetSubOrgs (List<string> orgs, string org) {
	  List<string> workers = GetWorkers(org);
	  List<string> managers = orgs.Select(GetManager).ToList();
	  List<int> indexes = managers.Select(s => workers.IndexOf(s)).ToList();
	  
	  List<string> subOrgs = new List<string>();
	  for (int i = 0; i < indexes.Count; i++)
	  {
		if (indexes[i] > -1)
			subOrgs.Add(orgs[i]);
	  }
	  return subOrgs;
	}
	
	public static int GetOrgSize (List<string> orgs, string org) {
	  List<string> workers = GetWorkers(org);
	  List<string> subOrgs = GetSubOrgs(orgs, org);
	  if (subOrgs.Count == 0) {
		return workers.Count;
	  } else {
		int size = 0;
		for (int i = 0; i < subOrgs.Count; i++)
		{
			size += GetOrgSize(subOrgs, subOrgs[i]) - 1;
		}
		return workers.Count + size;
	  }
	}
}
