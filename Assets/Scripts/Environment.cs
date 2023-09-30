using System;

[Serializable]
public class Environment
{
    public int position;
    public int leftPlayerTime;
    public int rightPlayerTime;

    public Environment(int position, int leftPlayerTime, int rightPlayerTime)
    {
        this.position = position;
        this.leftPlayerTime = leftPlayerTime;
        this.rightPlayerTime = rightPlayerTime;
    }

}
