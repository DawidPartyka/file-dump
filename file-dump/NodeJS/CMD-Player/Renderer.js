const spawnProcess = require('child_process').spawn;
const PngImage = require('node-libpng').PngImage;
const fs = require('fs');
const path = require('path');
//const sig = require('signal');
const ffmpegPath = './ffmpeg/bin/ffmpeg.exe';
let fp, w, h;

process.argv.forEach((entry, id, arr) => {
  if(id == 2) fp = entry;
  if(id == 3) h = entry;
  if(id == 4) w = entry;
});

class frameHandler{
  constructor(fw, fh, con = 1){
    this.frameWidth = fw;
    this.frameHeight = fh;
    this.bufferQueue = [];
    this.PNGFrameQueue = [];
    this.maxBufferConcatenation = con;
  }

  async pushBuffer(x){
    this.bufferQueue.push(x);
  }

  async getFrameFromBuffer(){
    let buff;

    for(let i = 0, max = this.maxBufferConcatenation; i < max; i++){
      buff += this.bufferQueue.shift(); //fifo

      try{
        const frame = new PngImage(buff);

        if(frame)
          return frame;
      }
      catch(e){
        fs.appendFile(`exceptionLog-${new Date(Date.now()).toDateString()}.txt`, e,
        (err) => {
          if(err)
            console.log(err);
        });
      }
    }

    return null;
  }

  async frameToGrayScale(brightnessChars = [' ', '@'], fr = this.PNGFrameQueue.shift()){
    const brightnessArr = [];
    const len = brightnessChars.length;
    const valueDivide = (256 / len).toFixed(2);

    for(let i = 0, w = this.frameWidth; i < w; i++){
      for(let j = 0, h = this.frameHeight; j < h; j++){
        let pixel = fr.rgbaAt(o, t);

        const brightVal = Math.round(( (0.21 * pixel[0]) + (0.72 * pixel[1]) + (0.07 * pixel[2]) )) / valueDivide;

        brightnessArr.push( brightnessChars[brightVal > len ? len : brightVal] );
      }
    }

    return brightnessArr;
  }
}

class cmdHandler{
  constructor(){
      this.cleared = true;
      this.cmdChars = [' ', '.', ',', '-', '~', ':', ';', '=', '!', '*', '#', '$', '@'];
  }

  async pushString(str){

  }

  async brightnessToChar(){

  }

  async getFrame(){

  }

  async display(frame){
    this.cleared = false;
  }

  async clear(){
    this.cleared = false;

    let cl = process.stdout.write('\x1b[2J'); // cleares console

    if (!cl) {
      process.stdout.on('drain', _);
    }

    this.cleared = true;
  }

  async prepareFrame(frameHandler){
    let fr = await frameHandler.frameToGrayScale(this.cmdChars);
    let frString = "";

    while(fr){
      const chars =
      frString += x.slice(0, x.length > frameHandler.frameWidth ? frameHandler.frameWidth : x.length) + '\n';
    }

    return frString
  }

  async update(frameHandler){
    if(!this.cleared)
      return;

    if(this.frameQueue.length){
      let x = await this.prepareFrame(frameHandler);
      display(x);
    }
  }
}


const screenVals = {
  h: null,
  w: null,
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
      //control = false;
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
  clear: async function(){
    /*let cl = process.stdout.write('\x1b[2J'); // cleares console

    if (!cl) {
      process.stdout.on('drain', _);
    }*/
    //console.log('\033[2J');
    process.stdout.write('\x1b[2J');
    process.stdout.write('\033[2J');
    //console.clear();
   },
  testLog: function(){for(let y = 0; y < this.h; y++) console.log('#'.repeat(this.w));}
}

const logStream = fs.createWriteStream('./logFile.log');

class render{
  constructor(UserFilePath, height, width){
    this.filePath = UserFilePath;
    screenVals.h = width;
    screenVals.w = height;
    this.lastBuf = null;

    console.log(this.filePath, screenVals.h, screenVals.w);
  }

  getPNGLength(x){

  }

  async  start(){
    return new Promise(async (resolve, reject) => {
    const resolution = screenVals.w + "*" + screenVals.h;

    const ffmpeg = spawnProcess(ffmpegPath, [
            '-re', '-i', /*'-re',*/ this.filePath, '-vf', 'transpose',
            '-vcodec', 'png',
            '-f', /*"transpose=2", */'rawvideo', /*'-r', '15',*/
            //'-s', `80*250`, // size of one frame
            '-s', `${resolution}`, // size of one frame
            'pipe:1'
        ]);

    ffmpeg.stderr.pipe(logStream); // for debugging

    let i = 0
    //let j = 0;
    //screenVals.testLog();
    //console.clear();

    ffmpeg.stdout.on('data', async(data) => {
        try {
          //console.log(data[0], data[1], data[2], data[3], data.length);
          //this.lastBuf = this.lastBuf ? Buffer.concat([this.lastBuf, data]): data;
          //console.log(this.lastBuf);
          //console.log(this.lastBuf.length);
          /*if("" + data[0] + data[1] + data[2] + data[3] !== "13780787113102610"){
            if(!this.lastBuf){
              this.lastBuf = data;
              ++j;
              return;
            }

            else if(await screenVals.test(Buffer.concat([this.lastBuf, data])) != null){
              await screenVals.clear();
              await screenVals.display();
              console.log(++i, j);
            }
            this.lastBuf = null;
          }
          else{
            this.lastBuf = data;
            ++j;
          }*/


          let fr = screenVals.test(data);
          //let fr = screenVals.test(this.lastBuf);

          //if(screenVals.test(this.lastBuf) != undefined){
          if(fr != undefined){
            //this.lastBuf = null;
            await screenVals.clear();
            await screenVals.display();
            console.log(`Frame ${++i}\n`);
          }

        } catch(e) {
            console.log(e)
        }
    });

    ffmpeg.on('exit', (c) => {
      console.log(`Process closed after generating ${i} frames and with code: ${c}`);
    });

      return true;
    });
    //init();
  }
}

console.log(fp, h, w);
console.clear();
const renderInstance = new render(fp, h, w);
renderInstance.start();
