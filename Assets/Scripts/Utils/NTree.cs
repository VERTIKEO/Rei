using System.Collections;
using System.Collections.Generic;

public delegate void TreeVisitor<T>(T parentNode, T childNode);

public class NTree<T>
{
    private T data;
    private LinkedList<NTree<T>> children;

    public NTree(T data)
    {
        this.data = data;
        children = new LinkedList<NTree<T>>();
    }

    public void AddChild(T data)
    {
        children.AddFirst(new NTree<T>(data));
    }

    public NTree<T> GetChild(int i)
    {
        foreach (NTree<T> n in children)
            if (--i == 0)
                return n;
        return null;
    }

    public void Traverse(TreeVisitor<T> visitor)
    {
        foreach (NTree<T> child in this.children) {
            visitor(this.data, child.data);
            Traverse(visitor);
        }
    }
}