import numpy as np
import pandas as pd
import nltk
import re
from nltk.tokenize import sent_tokenize
from nltk.corpus import stopwords
from sklearn.metrics.pairwise import cosine_similarity
import networkx as nx


# function to remove stopwords
def remove_stopwords(sen):
    stop_words = stopwords.words('english')
    sen_new = " ".join([i for i in sen if i not in stop_words])
    return sen_new


def get_sent_vectors(clean_sentences, word_embeddings):

    sentence_vectors = []
    for i in clean_sentences:
        if len(i) != 0:
            v = sum([word_embeddings.get(w, np.zeros((100,))) for w in i.split()]) / (len(i.split()) + 0.001)
        else:
            v = np.zeros((100,))
        sentence_vectors.append(v)

    return sentence_vectors


def get_similarity_matrix(sentences, sentence_vectors):

    # similarity matrix
    sim_mat = np.zeros([len(sentences), len(sentences)])

    for i in range(len(sentences)):
        for j in range(len(sentences)):
            if i != j:
                sim_mat[i][j] = \
                    cosine_similarity(sentence_vectors[i].reshape(1, 100), sentence_vectors[j].reshape(1, 100))[0, 0]

    return sim_mat


def get_page_rank(sim_mat):

    nx_graph = nx.from_numpy_array(sim_mat)
    scores = nx.pagerank(nx_graph)

    return scores


def get_summary(scores, sentences, manual_range, tags, separated_tags):

    ranked_sentences = sorted(((scores[i], s) for i, s in enumerate(sentences)), reverse=True)

    final_sentences = []

    num_sentences = len(ranked_sentences)

    summary_length = int((manual_range / 100) * num_sentences)

    tagged_sentences = {}

    for i in range(summary_length):
        final_sentences.append(ranked_sentences[i][1]) #if no tags just use ranking

        if tags: #if tags then only select sentences with tags in them
            for sentence in final_sentences:
                for tag in separated_tags:
                    if tag in sentence:
                        if sentence not in tagged_sentences.keys():
                            tagged_sentences[sentence] = 1
                        else:
                            tagged_sentences[sentence] += 1

    return final_sentences, num_sentences, tagged_sentences


def shorttext(input_text, word_embeddings, manual_range, tags):

    separated_tags = [x.strip() for x in tags.split(',')]

    sentences = [sent_tokenize(input_text)]

    sentences = [y for x in sentences for y in x]  # flatten list

    # remove punctuations, numbers and special characters
    clean_sentences = pd.Series(sentences).str.replace("[^a-zA-Z]", " ")

    # make alphabets lowercase
    clean_sentences = [s.lower() for s in clean_sentences]

    # remove stopwords from the sentences
    clean_sentences = [remove_stopwords(r.split()) for r in clean_sentences]

    sentence_vectors = get_sent_vectors(clean_sentences, word_embeddings)
    sim_mat = get_similarity_matrix(sentences, sentence_vectors)
    scores = get_page_rank(sim_mat)
    final_sentences, num_sentences, tagged_sentences = get_summary(scores, sentences, manual_range, tags, separated_tags)

    return final_sentences, input_text, len(final_sentences), num_sentences, tagged_sentences


