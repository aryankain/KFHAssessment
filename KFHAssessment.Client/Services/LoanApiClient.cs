using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using KFHAssessment.Shared.DTOs;

namespace KFHAssessment.Client.Services;

public class LoanApiClient
{
    private readonly HttpClient _http;
    private readonly TokenStore _store;

    public LoanApiClient(HttpClient http, TokenStore store)
    { _http = http; _store = store; }

    private void Auth()
    {
        if (!string.IsNullOrEmpty(_store.Token))
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _store.Token);
    }

    public async Task<List<LoanDto>> GetLoansAsync()
    {
        Auth();
        return await _http.GetFromJsonAsync<List<LoanDto>>("api/loans") ?? new();
    }

    public async Task<LoanDto?> CreateLoanAsync(CreateLoanDto dto)
    {
        Auth();
        var r = await _http.PostAsJsonAsync("api/loans", dto);
        r.EnsureSuccessStatusCode();
        return await r.Content.ReadFromJsonAsync<LoanDto>();
    }

    public async Task<LoanEvaluationResultDto?> EvaluateAsync(int id)
    {
        Auth();
        var r = await _http.PostAsync($"api/loans/{id}/evaluate", null);
        if (r.StatusCode == HttpStatusCode.NotFound) return null;
        r.EnsureSuccessStatusCode();
        return await r.Content.ReadFromJsonAsync<LoanEvaluationResultDto>();
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
    {
        var r = await _http.PostAsJsonAsync("api/auth/login", dto);
        if (!r.IsSuccessStatusCode) return null;
        return await r.Content.ReadFromJsonAsync<LoginResponseDto>();
    }
}