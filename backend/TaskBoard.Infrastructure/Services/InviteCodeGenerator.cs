using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Interfaces;

namespace TaskBoard.Infrastructure.Services;

public class InviteCodeGenerator : IInviteCodeGenerator
{
    private readonly char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
    private const int inviteCodeLength = 8;
    public readonly IApplicationDbContext _context;

    public InviteCodeGenerator(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<string> GenerateUniqueInviteCode(CancellationToken cancellationToken)
    {
        string code;
        bool exists;

        do
        {
            code = Generate();
            exists = await _context.Boards.AnyAsync(b => b.InviteCode == code, cancellationToken: cancellationToken);
        }
        while (exists);

        return code;
    }

    public string Generate()
    {
        var data = new byte[inviteCodeLength];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(data);

        var result = new char[inviteCodeLength];

        for (int i = 0; i < inviteCodeLength; i++)
        {
            result[i] = chars[data[i] % chars.Length];
        }

        return new string(result);
    }

}