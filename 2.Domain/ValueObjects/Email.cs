using System.Text.RegularExpressions;

namespace _2.Domain.ValueObjects;

public class Email
{
    public string Value { get; set; }
    private Email() { }
    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email no puede estar vacío.");

        email = email.Trim().ToLower();

        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("El formato del email es inválido.");

        return new Email(email);
    }

    public override string ToString() => Value;
    
    public override bool Equals(object obj)
    {
        return obj is Email other && Value == other.Value;
    }

    public override int GetHashCode() => Value.GetHashCode();
}