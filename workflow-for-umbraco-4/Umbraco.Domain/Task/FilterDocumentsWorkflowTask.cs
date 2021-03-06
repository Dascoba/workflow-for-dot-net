﻿using System;
using System.Collections.Generic;
using FergusonMoriyam.Workflow.Domain.Task;
using FergusonMoriyam.Workflow.Interfaces.Application.Runtime;
using FergusonMoriyam.Workflow.Interfaces.Domain;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;

namespace FergusonMoriyam.Workflow.Umbraco.Domain.Task
{
    [Serializable]
    public class FilterDocumentsWorkflowTask : BaseWorkflowTask, IWorkflowTask, IRunnableWorkflowTask
    {
        public IList<int> DocumentTypes { get; set; }

        public FilterDocumentsWorkflowTask() : base()
        {
            AvailableTransitions.Add("contains_docs");
            AvailableTransitions.Add("does_not_contain_docs");
        }

        public void Run(IWorkflowInstance workflowInstance, IWorkflowRuntime runtime)
        {
            // Cast to Umbraco worklow instance.
            var umbracoWorkflowInstance = (UmbracoWorkflowInstance) workflowInstance;

            var count = 0;
            var newCmsNodes = new List<int>();

            foreach(var nodeId in umbracoWorkflowInstance.CmsNodes)
            {
                var n = new CMSNode(nodeId);
                if(!n.IsDocument()) continue;

                var d = new Document(nodeId);
                if (!DocumentTypes.Contains(d.ContentType.Id)) continue;
                
                newCmsNodes.Add(nodeId);
                count++;
            }

            umbracoWorkflowInstance.CmsNodes = newCmsNodes;

            var transition = (count > 0) ? "contains_docs" : "does_not_contain_docs";
            runtime.Transition(workflowInstance, this, transition);
        }
    }
}
