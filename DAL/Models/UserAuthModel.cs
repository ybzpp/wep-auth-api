﻿namespace WebAuth.DAL.Models;

public class UserAuthModel
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
}