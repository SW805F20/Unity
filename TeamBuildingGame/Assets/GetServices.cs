using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zeroconf;

public class GetServices : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task EnumerateAllServicesFromAllHosts()
    {
        ILookup<string, string> domains = await ZeroconfResolver.BrowseDomainsAsync();
        var responses = await ZeroconfResolver.ResolveAsync(domains.Select(g => g.Key));
        foreach (var resp in responses)
            Debug.Log(resp);
    }
}
