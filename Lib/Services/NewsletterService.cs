﻿using Core.Consts;
using Core.Dtos.Footnote;
using Core.Dtos.Newsletter;
using Core.Dtos.User;
using Core.Models.Options;
using Microsoft.Extensions.Options;

namespace Lib.Services;

public class NewsletterService
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<SiteSettings> _siteSettings;

    public NewsletterService(IHttpClientFactory httpClientFactory, IOptions<SiteSettings> siteSettings)
    {
        _siteSettings = siteSettings;
        _httpClient = httpClientFactory.CreateClient();
        if (_httpClient.BaseAddress != _siteSettings.Value.ApiUri)
        {
            _httpClient.BaseAddress = _siteSettings.Value.ApiUri;
        }
    }

    public async Task<ApiResult<IList<FootnoteDto>>> GetFootnotes(UserNewsletterDto? user = null, int count = 1)
    {
        if (user == null)
        {
            var response = await _httpClient.GetAsync($"{_siteSettings.Value.ApiUri.AbsolutePath}/newsletter/Footnotes?count={count}");
            return await ApiResult<IList<FootnoteDto>>.FromResponse(response);
        }

        var userResponse = await _httpClient.GetAsync($"{_siteSettings.Value.ApiUri.AbsolutePath}/newsletter/Footnotes?count={count}&email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(user.Token)}");
        return await ApiResult<IList<FootnoteDto>>.FromResponse(userResponse);
    }

    public async Task<ApiResult<IList<FootnoteDto>>> GetUserFootnotes(UserNewsletterDto user, int count = 1)
    {
        var response = await _httpClient.GetAsync($"{_siteSettings.Value.ApiUri.AbsolutePath}/newsletter/Footnotes/Custom?count={count}&email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(user.Token)}");
        return await ApiResult<IList<FootnoteDto>>.FromResponse(response);
    }

    /// <summary>
    /// Root route for building out the the workout routine newsletter.
    /// </summary>
    public async Task<ApiResult<NewsletterDto>> Newsletter(string email = UserConsts.DemoUser, string token = UserConsts.DemoToken, DateOnly? date = null)
    {
        var response = await _httpClient.GetAsync($"{_siteSettings.Value.ApiUri.AbsolutePath}/newsletter/Newsletter?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}&date={date}&client={Client.App}");
        return await ApiResult<NewsletterDto>.FromResponse(response);
    }
}
