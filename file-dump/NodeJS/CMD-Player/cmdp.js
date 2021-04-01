const fs = require('fs');
const path = require('path');
const prompt = require('prompt');
const spawnProcess = require('child_process').spawn;
const readline = require('readline');
const kill = require('tree-kill');

function askQuestion(query) {
    const rl = readline.createInterface({
        input: process.stdin,
        output: process.stdout,
    });

    return new Promise(resolve => rl.question(query, ans => {
        rl.close();
        resolve(ans);
    }))
}

async function init(){
  let fp;
  let height = 80;
  let width = 250;

  console.clear();

  const p1 = await askQuestion('\nWrite full path to the file or the file name if it\'s in the same folder as the program \n(use "/" instead of "\\" and remember about the file extension):\n').then(
    async (res1) => {
      fp = res1;

      const p2 = await askQuestion('\nWrite the resoltion (Height-Width ie. 50-250) or leave blank for default 80-250 value:\n').then(
        async (res2) => {
          if(res2 != ''){
            height = res2.split('-')[0];
            width = res2.split('-')[1];
          }

          /*console.log(path.resolve(fp), height, width);
          const renderInstance = new render(path.resolve(fp), height, width);
          await renderInstance.start();*/
          let spawn = spawnProcess('cmd.exe', [
                  '/c', 'node', 'renderer2.js', fp,
                  //'/c', 'node', 'Renderer.js', fp,
                  height, width
              ], {shell: true, detached: true});

          const p3 = await askQuestion('\nChoose option number\n1. Close recently opened image/video\n2. Open another\n').then(
            async (res3) => {
              console.log(res3);
              if(res3 == '1'){
                spawn.stdin.pause();
                kill(spawn.pid);
              }

              init();
            }
          )

        }
      );
    }
  );
}

function chunk(str, n) {
    var ret = [];
    var i;
    var len;

    for(i = 0, len = str.length; i < len; i += n) {
       ret.push(str.substr(i, n))
    }

    return ret;
};


const screenVals = {
  h: 250,
  w: 80,
  signs: [' ', '.', ',', '-', '~', ':', ';', '=', '!', '*', '#', '$', '@'],
  signLen: 13,
  control: false,
  frame: "",
  calculate: function(x){
    return this.signs[Math.round((parseInt(x, 16) / 13) / 19.65)];
  },
  calculateRgba: function(x){
    let val = Math.floor(((0.21 * x[0]) + (0.72 * x[1]) + (0.07 * x[2])) / 19);
    //console.log(val);
    if(val > 12) val = 12;
    /*if(Math.round((parseInt(x, 16) / 13) / 18.95) > 15)
      console.log(Math.round((parseInt(x, 16) / 13) / 18.95));*/
    return this.signs[val];
  },
  test: function(x){
    try{
      control = false;
      let frame = new PngImage(x);
      //let testArr = [];
      this.frame = "";
      for(let o = 0; o < this.w; o++){
        //let row = [];
        for(let t = 0; t < this.h; t++){
          //testArr[t] = this.calculateRgba(frame.rgbaAt(o, t));
          //this.frame += this.calculateRgba(frame.rgbaAt(this.w * (this.h - 1 - t) + o));
          this.frame += this.calculateRgba(frame.rgbaAt(o, t));
        }

        this.frame += '\n';
      }

      return this.frame;
    }
    catch(e){
      /*--i;
      control = true;*/
    }
  },
  rot: function(){
    let newFrame = [];
    this.frame = this.frame.split('');
    //console.log("length" + this.frame.length);
    for (let q = 0; q < this.w; ++q) {
        for (let j = 0; j < this.h; ++j) {
            newFrame[this.h * q + j] = this.frame[this.w * (this.h - 1 - j) + q];
            //console.log(this.h * q + j);
        }

        //newFrame[this.h * q + this.w] = '\n';
    }


    for(let x = 1; x < this.w; x++){
      //console.log(`x:${this.frame[this.w * x]}`);
      newFrame[this.h * x] = '\n';
      //console.log(`x:${this.frame[this.w * x]}\n\n`);
    }

    this.frame = newFrame.join('');
  },
  addNewLines: function(){
    let newFrame = this.frame.split('');
    for(let x = 1; x < this.w; x++){
      //console.log(`x:${this.frame[this.w * x]}`);
      newFrame[this.h * x] = '\n';
      //console.log(`x:${this.frame[this.w * x]}\n\n`);
    }
    this.frame = newFrame.join('');
  },
  rotate: function(){
    /*let fr = [];
    let rl = Math.sqrt(this.frame.length);

    for(let w = 0; w < this.frame.length; w++){
      let x = w % rl;
      let y = Math.floor(w / rl);
      let nx = rl - y - 1;
      let ny = x;
      let np = ny * rl + nx;

      fr[np] = this.frame[x];
    }

    this.frame = fr.join('');
    console.log(fr);*/
    let newFrame = [];
    this.frame = this.frame.split('');

    for (let q = 0; q < this.w; ++q) {
        for (let j = 0; j < this.h; ++j) {
            newFrame[this.h * q + j] = this.frame[this.w * (this.h - 1 - j) + q];
        }

        //newFrame[this.h * q + this.w] = '\n';
    }

    this.frame = newFrame.join('');
    //console.log(newFrame);
  },
  display: async function(){console.log(this.frame); this.frame = "";},
  clear: async function(){ console.clear();},
  newlines: function(){this.frame = chunk(this.frame, this.w).join('\n');},
  testLog: function(){for(let y = 0; y < this.h; y++) console.log('#'.repeat(this.w));}
}

const logStream = fs.createWriteStream('./logFile.log');

class render{
  constructor(UserFilePath, height, width){
    this.filePath = UserFilePath;
    screenVals.h = width;
    screenVals.w = height;
  }

async  start(){
    return new Promise(async (resolve, reject) => {
    const resolution = screenVals.w + "*" + screenVals.h;

    const ffmpeg = spawnProcess('ffmpeg', [
            '-re', '-i', /*'-re',*/ this.filePath, '-vf', 'transpose',
            '-vcodec', 'png',
            '-f', /*"transpose=2", */'rawvideo', /*'-r', '15',*/
            //'-s', `80*250`, // size of one frame
            '-s', `${resolution}`, // size of one frame
            'pipe:1'
        ]);

    ffmpeg.stderr.pipe(logStream); // for debugging

    let i = 0
    //screenVals.testLog();
    //console.clear();

    ffmpeg.stdout.on('data', async(data) => {
        try {
          let fr = screenVals.test(data);

          if(fr != undefined){
            await screenVals.clear();
            await screenVals.display();
            console.log(`Frame ${++i}\n`);
          }

        } catch(e) {
            console.log(e)
        }
    });

    ffmpeg.stdout.on('close', function(chunk) {
        console.log('close');
        //init();
    });

      return true;
    });
    //init();
  }
}
/*
async function startWindow(){
  const renderInstance = new render(path.resolve('main.bmp'), 34, 250);
  await renderInstance.start();
}

startWindow();
setTimeout(init(), 3000);*/

init();
