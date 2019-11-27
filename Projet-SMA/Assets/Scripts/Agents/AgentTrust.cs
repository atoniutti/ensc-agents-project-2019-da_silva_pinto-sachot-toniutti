using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AgentTrust
{
    public Agent agent;
    public float trust;

    public AgentTrust(Agent agent, float trust)
    {
        this.agent = agent;
        this.trust = trust;
    }
}
