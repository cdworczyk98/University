from flask import Flask, redirect, url_for, render_template, request
from textSummarize import shorttext
from werkzeug.utils import secure_filename
from werkzeug.datastructures import FileStorage
import numpy as np

import os

app = Flask(__name__)
word_embeddings = {}


@app.route('/')
def home():
    return render_template("summary.html")


@app.route('/summary', methods=['POST'])
def summary():
    if request.method == 'POST':

        print(request.form['tags'])

        tags = ""

        if request.form['summary'] == 'automatic':
            summary_range = 50
        elif request.form['summary'] == 'manual':
            summary_range = int(request.form['manualRange'])
        elif request.form['summary'] == 'tag':
            summary_range = 50
            tags = request.form['tags']

        if request.form['inputtype'] == 'fromfile':
            # for secure filenames. Read the documentation.
            file = request.files['myfile']
            filename = secure_filename(file.filename)

            # os.path.join is used so that paths work in every operating system
            file.save(os.path.join("output", filename))

            # You should use os.path.join here too.
            with open(os.path.join("output", filename)) as f:
                final_text = f.read()

        elif request.form['inputtype'] == 'textbox':
            final_text = request.form['mytext']
        else:
            with open(os.path.join("output", "Text_File.txt")) as f:
                final_text = f.read()

        summary, raw_text, sumNumSent, numSent, tagged_sentences = shorttext(final_text, word_embeddings, summary_range, tags)

        if not tagged_sentences:
            return render_template("summary.html", fullText=raw_text, summaText=summary, sumNumSent=sumNumSent, numSent=numSent, sumPercent=(sumNumSent/numSent)*100)
        else:
            return render_template("summary.html", fullText=raw_text, summaText=tagged_sentences, sumNumSent=len(tagged_sentences), numSent=numSent, sumPercent=(len(tagged_sentences) / numSent) * 100)


if __name__ == '__main__':

    f = open('glove/glove.6B.100d.txt', encoding='utf-8')
    for line in f:
        values = line.split()
        word = values[0]
        coefs = np.asarray(values[1:], dtype='float32')
        word_embeddings[word] = coefs
    f.close()

    app.run(debug=True)
