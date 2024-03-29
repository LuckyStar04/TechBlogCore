﻿using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Entities;
using TechBlogCore.RestApi.Repositories;

namespace TechBlogCore.RestApi.Services
{
    public class ChatService
    {
        private readonly IConfiguration configuration;
        private readonly HttpClient httpClient;
        private readonly IChatRepo chatRepo;
        private readonly UserManager<Blog_User> userManager;
        public ChatService(IConfiguration configuration, HttpClient httpClient, IChatRepo chatRepo, UserManager<Blog_User> userManager)
        {
            this.configuration = configuration;
            this.httpClient = httpClient;
            this.chatRepo = chatRepo;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<ChatDto>> GetChatList(ClaimsPrincipal User)
        {
            var user = await userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email)?.Value);
            return chatRepo.GetMessagesByUserId(user.Id).Select(v => new ChatDto
            {
                isMe = v.IsMe,
                content = v.Message,
                time = v.Time,
            });
        }

        public string ChatComplete(ClaimsPrincipal User, ChatCompleteInputDto dto)
        {
            var user = userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email)?.Value).GetAwaiter().GetResult();
            var url = configuration["OpenAI:RequestURL"];
            var token = configuration["OpenAI:API_Key"];

            var messages = new List<ChatCompleteMessageDto>(10);
            if (!string.IsNullOrEmpty(dto.Role))
            {
                messages.Add(new ChatCompleteMessageDto { role = "system", content = dto.Role });
            }
            messages.AddRange(chatRepo.GetMessagesByUserId(user.Id).Select(v => new ChatCompleteMessageDto
            {
                role = v.IsMe ? "user" : "assistant",
                content = v.IsMe || string.IsNullOrEmpty(v.Message) ? v.Message : JsonSerializer.Deserialize<ChatResponseDto>(v.Message).choices[0].message.content,
            }));
            if (!string.IsNullOrEmpty(dto.Content))
            {
                messages.Add(new ChatCompleteMessageDto { role = "user", content = dto.Content });
                chatRepo.AddMessage(new MessageCreateDto
                {
                    Blog_UserId = user.Id,
                    IsMe = true,
                    Group = 0,
                    Time = DateTime.Now,
                    Role = dto.Role,
                    Message = dto.Content,
                });
            }

            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Content = JsonContent.Create<ChatCompleteDto>(new ChatCompleteDto
                {
                    model = "gpt-3.5-turbo",
                    messages = messages
                });
                var response = httpClient.Send(request);
                response.EnsureSuccessStatusCode();
                var stream = response.Content.ReadAsStream();
                var reader = new StreamReader(stream);
                var result = reader.ReadToEnd();
                chatRepo.AddMessage(new MessageCreateDto
                {
                    Blog_UserId = user.Id,
                    IsMe = false,
                    Group = 0,
                    Time = DateTime.Now,
                    Role = dto.Role,
                    Message = result,
                });
                return result;
            }

            //Thread.Sleep(1500);
            //var result = "{\"id\":\"chatcmpl-75zPlHsTYnLtMIcpMZ1kWe6pBUj1e\",\"object\":\"chat.completion\",\"created\":1681662073,\"model\":\"gpt-3.5-turbo-0301\",\"usage\":{\"prompt_tokens\":30,\"completion_tokens\":750,\"total_tokens\":780},\"choices\":[{\"message\":{\"role\":\"assistant\",\"content\":\"以下是一个简单的树形组件示例，其中使用了Vue.js和v-model来绑定所选项：\\n\\n```html\\n<template>\\n  <div class=\\\"tree\\\">\\n    <ul>\\n      <li v-for=\\\"item in items\\\">\\n        <label>\\n          <input type=\\\"checkbox\\\" v-model=\\\"item.checked\\\">\\n          {{ item.name }}\\n        </label>\\n        <tree v-if=\\\"item.children\\\" :items=\\\"item.children\\\" v-model=\\\"item.checked\\\"></tree>\\n      </li>\\n    </ul>\\n  </div>\\n</template>\\n\\n<script>\\nexport default {\\n  name: 'tree',\\n  props: {\\n    items: Array, // 树节点数据\\n    value: Boolean // 绑定值\\n  },\\n  data() {\\n    return {\\n      checkAll: false // 全选\\n    };\\n  },\\n  computed: {\\n    // 是否所有节点都已选择\\n    allChecked() {\\n      return this.items.every((item) => item.checked);\\n    },\\n    // 是否部分节点已选择\\n    partialChecked() {\\n      return !this.allChecked && this.items.some((item) => item.checked);\\n    },\\n  },\\n  watch: {\\n    // 监听全选按钮\\n    checkAll() {\\n      this.items.forEach((item) => (item.checked = this.checkAll));\\n    },\\n    // 监听子节点选择更新父节点\\n    items: {\\n      handler(newValue) {\\n        if (newValue.every((item) => item.checked)) {\\n          this.$emit('input', true);\\n        } else if (newValue.some((item) => item.checked)) {\\n          this.$emit('input', null);\\n        } else {\\n          this.$emit('input', false);\\n        }\\n      },\\n      deep: true\\n    },\\n    // 监听绑定值更新子节点选择\\n    value: function (newValue) {\\n      this.items.forEach((item) => (item.checked = newValue));\\n    }\\n  }\\n};\\n</script>\\n\\n<style>\\n  .tree {\\n    border: 1px solid #ccc;\\n    padding: 10px;\\n  }\\n  ul {\\n    list-style: none;\\n    margin: 0;\\n    padding: 0;\\n  }\\n  li {\\n    margin-left: 20px;\\n    margin-top: 5px;\\n  }\\n</style>\\n```\\n\\n在父组件中使用该组件时，可以通过v-model来绑定所选项的值：\\n\\n```\\n<template>\\n  <div>\\n    <tree :items=\\\"treeData\\\" v-model=\\\"selected\\\"></tree>\\n    <div>已选择：{{selected}}</div>\\n  </div>\\n</template>\\n\\n<script>\\nimport Tree from './Tree';\\nexport default {\\n  name: 'tree-demo',\\n  components: {\\n    Tree\\n  },\\n  data() {\\n    return {\\n      treeData: [\\n        {\\n          name: '节点1',\\n          checked: false,\\n          children: [\\n            { name: '节点1-1', checked: false },\\n            { name: '节点1-2', checked: false }\\n          ]\\n        },\\n        {\\n          name: '节点2',\\n          checked: false,\\n          children: [\\n            {\\n              name: '节点2-1',\\n              checked: false,\\n              children: [\\n                { name: '节点2-1-1', checked: false },\\n                { name: '节点2-1-2', checked: false }\\n              ]\\n            }\\n          ]\\n        }\\n      ],\\n      selected: false // 绑定值\\n    };\\n  }\\n};\\n</script>\\n```\"},\"finish_reason\":\"stop\",\"index\":0}]}";
            //return Content(result, "application/json");
        }
    }
}
