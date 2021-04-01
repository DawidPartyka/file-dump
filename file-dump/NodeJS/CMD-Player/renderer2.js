const { spawn, exec } = require('child_process');

const PngImage = require('node-libpng').PngImage;
const fs = require('fs');
const path = require('path');
const ffmpegPath = './ffmpeg/bin/ffmpeg.exe';

let fp, w, h;

fp = process.argv[2];
h = process.argv[3];
w = process.argv[4];

class frameHandler{
  BufferQueue = [];
  CharQueue = [];
  FrameStringQueue = [];

  constructor(fw, fh, con = 1, charArr){
    this.frameWidth = fw;
    this.frameHeight = fh;
    //this.BufferQueue = [];
    //this.CharQueue = [];
    //this.FrameStringQueue = [];
    this.working = false;
    this.forceStop = false;
    this.charArr = charArr;
    this.frameCounter = 0;
    this.badFrameCounter = 0;
    this.divide = (256 / charArr.length).toFixed(2);
    this.maxBufferConcatenation = con;
  }

  async pushBuffer(x){
    //console.log('received buffer');
    this.BufferQueue.push(x);
    this.processBuffer();
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

  async calculateBrightness(rgb){
    let val = Math.floor(((0.21 * rgb[0]) + (0.72 * rgb[1]) + (0.07 * rgb[2])) / this.divide);
    //console.log(val);
    if(val > this.charArr.length - 1) val = this.charArr.length - 1;

    return this.charArr[val];
  }

  async display(str){

    const s = str?.shift();

    if(s){
      exec('cls');
      //process.stdout.write('\x1b[2J');
      //process.stdout.write('\033[2J');
      //process.stdout.write('cls');
      //console.clear();
      //console.log(this.FrameStringQueue);
      //setTimeout(function(){
      console.log(s);
      console.log(`Frame ${++this.frameCounter}\tBFC ${this.badFrameCounter}`);
    }
    //else
      //console.log(`Frame ${this.frameCounter}\tBFC ${this.badFrameCounter}`);
      //console.log('framedisplay\n',str + `\n\nFrame ${++this.frameCounter}\tBFC ${this.badFrameCounter}`);
      //console.log('framedisplay\n',this.FrameStringQueue.shift() + `\n\nFrame ${++this.frameCounter}\tBFC ${this.badFrameCounter}`);
    //}, 10);
  }

  async processFrame(){
    //console.log('cq', this.CharQueue, this.CharQueue.length);
    if(!this.CharQueue.length)
      return;

    let f = this.CharQueue.shift();
    this.FrameStringQueue.push(f.join(''));
  }

  async processBuffer(){
    //console.log('pb');
    try{
      let frame = new PngImage(this.BufferQueue.shift()); //fifo
      //console.log(frame);
      //console.log(this.BufferQueue.length);

      if(!frame){
        //console.log(`Bad Frame at frame ${this.frameCounter}\nBFC: ${++this.badFrameCounter}`);
        return;
      }

      let charArr = [];
      let framestring = "";
      for(let i = 0; i < this.frameWidth; ++i){
        for(let j = 0; j < this.frameHeight; ++j){
          //charArr.push();
          framestring += await this.calculateBrightness(frame.rgbaAt(i, j));
        }
        framestring += '\n';
        //charArr.push('\n');
      }

      if(framestring.length){
        this.FrameStringQueue.push(framestring);
        const p1 = this.display(this.FrameStringQueue);
        Promise.all([p1]).catch(err => console.log(err));
        //console.log(this.FrameStringQueue);
      //if(charArr.length){
        //console.log('charr', charArr);
        //this.CharQueue.push(charArr);
        //this.FrameStringQueue.push(charArr.join(''));
        //console.log(this.CharQueue);
        //console.log(this.FrameStringQueue);
        //console.log(charArr.join(''));
      }

    }
    catch(e){
      ++this.badFrameCounter;
      //console.log(`Something went wrong at frame ${this.frameCounter}\nBFC: ${++this.badFrameCounter}`,e);
    }
  }

  async checkContinue(){
    //console.log('checking');
    //console.log(this.BufferQueue.length);
    if(this.forceStop && this.BufferQueue.length === 0)
      return;

    const prom = [];
    const p1 = this.processBuffer();
    //const p2 = this.processFrame();
    //const p3 = this.display();
    if(this.BufferQueue) this.processBuffer();
    //if(this.BufferQueue.length)
      //prom.push(p1);

    /*if(this.CharQueue.length)
      prom.push(p2);*/

    //if(this.FrameStringQueue.length)
    //  prom.push(p3);

    //console.log(prom, 'fsql', this.FrameStringQueue.length);
    if(prom.length)
      Promise.all(prom).then(await this.checkContinue()).catch(err => console.log(err));
    else{
      const clb = async() => {
        //console.log(this.checkContinue);
        await this.checkContinue();
      }
      //console.log(clb);
      setTimeout(clb, 1000);
    }

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

    return this.signs[val];
  },
  test: function(x){
    try{
      let frame = new PngImage(x);

      this.frame = "";
      for(let o = 0; o < this.w; o++){

        for(let t = 0; t < this.h; t++){
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
  addNewLines: function(){
    let newFrame = this.frame.split('');
    for(let x = 1; x < this.w; x++){
      //console.log(`x:${this.frame[this.w * x]}`);
      newFrame[this.h * x] = '\n';
      //console.log(`x:${this.frame[this.w * x]}\n\n`);
    }
    this.frame = newFrame.join('');
  },

  display: async function(){console.log(this.frame); this.frame = "";},
  clear: async function(){
    process.stdout.write('\x1b[2J');
    process.stdout.write('\033[2J');
    console.clear();
   }
}

const logStream = fs.createWriteStream('./logFile.log');

/*
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
            '-re', '-i', this.filePath, '-vf', 'transpose',
            '-vcodec', 'png',
            '-f', 'rawvideo',
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
*/

function startFFMPEG(filep, r){
  const resolution = r.frameWidth + "*" + r.frameHeight;

  const ffmpeg = spawn(ffmpegPath, [
          '-re', '-i', /*'-re',*/ filep, '-vf', 'transpose',
          '-vcodec', 'png',
          '-f', /*"transpose=2", */'rawvideo', /*'-r', '15',*/
          //'-s', `80*250`, // size of one frame
          '-s', resolution, // size of one frame
          'pipe:1'
      ]);

  ffmpeg.stderr.pipe(logStream); // for debugging

  ffmpeg.stdout.on('data', async(data) => {
    //console.log(data);
      r.pushBuffer(data);
  });

  ffmpeg.on('exit', (c) => {
    r.forceStop = true;
    console.log(`Process closed after generating ${r.frameCounter} frames and with code: ${c}`);
  });
}

console.log(fp, h, w);
console.clear();
//const renderInstance = new render(fp, h, w);
//renderInstance.start();
const r = new frameHandler(w, h, 1, [' ', '.', ',', '-', '~', ':', ';', '=', '!', '*', '#', '$', '@']);

//const p1 = ()=>{};
//const p1 = r.checkContinue();
//const p2 = startFFMPEG(fp, r);
//Promise.all([p1, p2]).catch(err => console.log(err));
//r.checkContinue();
const cmdw = ++w;
exec(`mode con: cols=${h} lines=${cmdw}`)
startFFMPEG(fp, r);
