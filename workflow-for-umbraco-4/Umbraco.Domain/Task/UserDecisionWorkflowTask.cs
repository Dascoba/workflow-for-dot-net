﻿using System;
using System.Collections.Generic;
using FergusonMoriyam.Workflow.Interfaces.Domain;
using umbraco.BusinessLogic;

namespace FergusonMoriyam.Workflow.Umbraco.Domain.Task
{
    [Serializable]
    public class UserDecisionWorkflowTask : BaseDecisionWorkflowTask, IWorkflowTask, IDecisionWorkflowTask
    {
        public IList<int> Users { get; set; }

        public UserDecisionWorkflowTask()
            : base()
        {
            AvailableTransitions.Add("approve");
            AvailableTransitions.Add("reject");
        }

        public override bool CanTransition()
        {
            if (Users != null)
            {
                if (Users.Contains(User.GetCurrent().Id))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
