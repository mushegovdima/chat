<script setup lang="ts">
import { HubConnectionBuilder, LogLevel, HubConnection, HttpTransportType } from '@microsoft/signalr';
import { ref, onMounted } from 'vue'

interface Message {
  userName: string;
  description: string;
  timestamp: number;
}

const user = ref<string>();
const text = ref<string>();
const connection = ref<HubConnection>();
const messages = ref<Message[]>([]);

onMounted(() => {
  connection.value = new HubConnectionBuilder()
      .withUrl(`http://localhost:4000/hubs/chat`,
        { 
          headers: { "access-control-allow-origin" : "*"},
          skipNegotiation: true,
          transport: HttpTransportType.WebSockets,
        })
      .configureLogging(LogLevel.Information)
      .build();

  connection.value
    .start()
    .then(() => {
      connection.value!.on("ReceiveMessage", (message: Message) => {
        messages.value.push(message);
      });
    })
    .catch((error: any) => console.log(error));
})

const sendMessage = async function (): Promise<void> {
  if (connection.value){
    const msg = { 
      userName: user.value,
      description: text.value,
      timestamp: new Date().getTime(),
    } as Message;
    await connection.value.send("SendMessage", msg);
    text.value = '';
  }
}

</script>

<template>
  <div class="chat-container">
    <div class="chat-messages" ref="chatMessages">
      <div v-for="(message, index) in messages" :key="index" class="chat-message">
        <div>
          <strong>{{ message.userName }}:</strong> {{ message.description }}
        </div>
        <div>
          {{ new Date(message.timestamp).toLocaleString() }}
        </div>
      </div>
    </div>

    <div class="chat-input mt-1">
      <div>
        <input
          v-model="user"
          type="text"
          placeholder="Введите ваше имя"
          class="username-input"
        />
      </div>
      <div class="d-flex w-full mt-1">
        <textarea
          v-model="text"
          type="textarea"
          placeholder="Введите сообщение"
          class="message-input"
          @keyup.enter="sendMessage"
        />
        <button @click="sendMessage" :disabled="!text">Отправить</button>
      </div>
    </div>
  </div>
</template>

<style scoped>
  .d-flex {
    display: flex;
  }
  .w-full {
    width: 100%;
  }
  .message-input {
    width: 80%;
    margin-right: 2em;
  }
  .mt-1 {
    margin-top: 1em;
  }

  .chat-message {
    display: flex;
    justify-content: space-between;
  }

</style>
