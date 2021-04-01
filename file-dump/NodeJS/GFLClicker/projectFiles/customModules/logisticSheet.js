const logisticSheet = {
  chp0: {
    m0: 50,
    m1: 180,
    m2: 720,
    m3: 1440
  },
  chp1: {
    m0: 15,
    m1: 30,
    m2: 60,
    m3: 120
  },
  chp2: {
    m0: 40,
    m1: 90,
    m2: 240,
    m3: 360
  },
  chp3: {
    m0: 20,
    m1: 45,
    m2: 90,
    m3: 300
  },
  chp4: {
    m0: 60,
    m1: 120,
    m2: 360,
    m3: 480
  },
  chp5: {
    m0: 30,
    m1: 150,
    m2: 240,
    m3: 420
  },
  chp6: {
    m0: 120,
    m1: 180,
    m2: 300,
    m3: 720
  },
  chp7: {
    m0: 150,
    m1: 240,
    m2: 330,
    m3: 480
  },
  chp8: {
    m0: 60,
    m1: 180,
    m2: 360,
    m3: 540
  },
  chp9: {
    m0: 30,
    m1: 90,
    m2: 270,
    m3: 420
  },
  chp10: {
    m0: 40,
    m1: 100,
    m2: 320,
    m3: 600
  },
  chp11: {
    m0: 240,
    m1: 240,
    m2: 480,
    m3: 600
  },
  chp12: {
    m0: 60,
    m1: 90,
    m2: 540,
    m3: 720
  },
  chp13: {
    m0: 180,
    m1: 360,
    m2: 1440,
    m3: 360
  },
  returnTimer: (missionCode) => {
    const data = missionCode.split('-');
    return logisticSheet[`chp${data[0]}`][`m${data[1]-1}`];
  }
}
module.exports = logisticSheet;
