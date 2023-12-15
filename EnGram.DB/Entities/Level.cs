﻿namespace EnGram.DB.Entities;

public class Level
{
    public int Id { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public ICollection<Topic> Topics { get; set; }
}