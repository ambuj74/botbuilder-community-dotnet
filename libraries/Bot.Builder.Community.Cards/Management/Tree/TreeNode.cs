﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Builder.Community.Cards.Management.Tree
{
    internal class TreeNode<TValue, TChild> : ITreeNode
        where TValue : class
        where TChild : class
    {
        public TreeNode(ChildCallerDelegate<TValue, TChild> childCaller)
        {
            ChildCaller = childCaller;
        }

        public PayloadIdType? IdType { get; set; }

        private ChildCallerDelegate<TValue, TChild> ChildCaller { get; }

        public async Task<object> CallChild(object value, Func<object, TreeNodeType, Task<object>> nextAsync)
        {
            // This check will prevent child callers from needing to check for nulls
            if (value is TValue typedValue)
            {
                return await ChildCaller(
                    typedValue,
                    async (child, childType) =>
                    {
                        return await nextAsync(child, childType).ConfigureAwait(false) as TChild;
                    }).ConfigureAwait(false);
            }

            return value;
        }

        public Type GetTValue()
        {
            return typeof(TValue);
        }
    }
}
