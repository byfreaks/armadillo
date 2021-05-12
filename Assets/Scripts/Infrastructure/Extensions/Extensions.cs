using System.Collections.Generic;

public static class ListExtension
{
    public static T PopAt<T>(this List<T> list, int index)
    {
        if(list.Count > 0){
            T r = list[index];
            list.RemoveAt(index);
            return r;
        } else {
            throw new System.ArgumentOutOfRangeException();
        }
        
    }

    public static T Pop<T>(this List<T> list) => list.PopAt(0);

    public static T PopLast<T>(this List<T> list) => list.PopAt(list.Count);
}

