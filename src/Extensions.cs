using System.Collections.Generic;
using Model.OpsNew;

namespace better_ui_mod;

public static class Extensions
{
	public static Dictionary<string, float> GetCapacities(this Industry anIndustry)
	{
		var contractMultiplier = anIndustry.GetContractMultiplier();
		Dictionary<string, float> capacities = new();
		
		foreach (var unloader in anIndustry.GetComponentsInChildren<IndustryUnloader>())
		{
			capacities.Add(unloader.load.id, unloader.maxStorage * contractMultiplier);
		}

		return capacities;
	}
}