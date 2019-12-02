using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AgentTrust
{
    public Agent _agent;
    public float _trust;

    public AgentTrust(Agent agent, float trust)
    {
        this._agent = agent;
        this._trust = trust;
    }
}
