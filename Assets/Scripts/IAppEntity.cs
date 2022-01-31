using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAppEntity
{
    BasePervasiveApp ParentApp { get; }
    void Bind(BasePervasiveApp toBindApp);

    bool Rendering { get; }
}
