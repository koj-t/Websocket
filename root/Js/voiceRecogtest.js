/*
 * echotest.js
 *
 * Derived from Echo Test of WebSocket.org (http://www.websocket.org/echo.html).
 *
 * Copyright (c) 2012 Kaazing Corporation.
 */

//var url = "ws://localhost:4649/Echo";
//var url = "wss://localhost:4649/Echo";
var output;

function init () {
  output = document.getElementById ("output");
  //doWebSocket ();
  voiceRecog ();
}

function doWebSocket () {
  websocket = new WebSocket (url);
  websocket.onopen = function (evt) {onOpen (evt)};
  websocket.onclose = function (evt) {onClose (evt)};
  websocket.onmessage = function (evt) {onMessage (evt)};
  websocket.onerror = function (evt) {onError (evt)};
}

function onOpen (evt) {
  writeToScreen ("CONNECTED");
  send ("WebSocket Connected");
}
function onClose (evt) {writeToScreen ("DISCONNECTED");}

function onMessage (evt) {
  writeToScreen ('<span style="color: blue;">RESPONSE: ' + evt.data + '</span>');
  //websocket.close ();
}

function onError (evt) {
  writeToScreen('<span style="color: red;">ERROR: ' + evt.data + '</span>');
}

function send (message) {
  writeToScreen ("SENT: " + message);
  websocket.send (message);
}

function writeToScreen (message) {
  var pre = document.createElement ("p");
  pre.style.wordWrap = "break-word";
  pre.innerHTML = message;
  output.appendChild (pre);
}

window.addEventListener ("load", init, false);

function voiceRecog () {
      // WebSocket でサーバと接続
      var ws = new WebSocket('wss://localhost:4649/Echo');
      ws.onmessage = function (evt) {onMessage (evt)};

      // Web Speech API で音声認識
      var recognition = new webkitSpeechRecognition();
      // 連続音声認識
      recognition.continuous = true;
      // エラー表示
      recognition.onerror = function(err) {console.error(err);}
      // 無音停止時に自動で再開
      recognition.onaudioend = function() {
          recognition.stop();
          setTimeout(function() {
            recognition.start();
          }, 1000);
      }
      // 音声認識結果をサーバへ送信
      recognition.onresult = function(event) {
        for (var i = event.resultIndex; i < event.results.length; ++i) {
          var result = event.results[event.resultIndex][0].transcript;
          document.getElementById('result').innerHTML = result;
          ws.send(result);
        }
      }
      // 音声認識開始　
      recognition.start();
      writeToScreen("Voice Recognition Start");
}
